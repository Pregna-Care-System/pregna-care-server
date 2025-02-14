using Microsoft.EntityFrameworkCore;
using PregnaCare.Common.Enums;
using PregnaCare.Core.Models;
using PregnaCare.Infrastructure.Data;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class GrowthAlertService : IGrowthAlertService
    {
        private readonly PregnaCareAppDbContext _context;
        private readonly IChatGPTService _chatGPTService;

        public GrowthAlertService(PregnaCareAppDbContext context, IChatGPTService chatGPTService)
        {
            _context = context;
            _chatGPTService = chatGPTService;
        }

        public async Task<string> CheckGrowthAndCreateAlert(Guid userId, FetalGrowthRecord record)
        {
            var growthMetric = await _context.GrowthMetrics
                                             .AsNoTracking()
                                             .FirstOrDefaultAsync(g => g.Week == record.Week &&
                                                                       g.Name == record.Name &&
                                                                       g.IsDeleted == false);

            if (growthMetric == null)
            {
                var estimatedRange = await _chatGPTService.GetEstimatedRange(record.Name, record.Week ?? 0);
                growthMetric = new GrowthMetric
                {
                    Name = record.Name,
                    Week = record.Week,
                    MinValue = estimatedRange.MinValue,
                    MaxValue = estimatedRange.MaxValue
                };
            }

            var severity = GetSeverity(record.Value, growthMetric.MinValue, growthMetric.MaxValue);
            if (severity == SeverityEnum.Normal) return SeverityEnum.Normal.ToString();

            var issuePrompt = $@"
                                A new fetal growth record has been added with the following details:
                                - **Measurement Name**: {record.Name}
                                - **Measurement Value**: {record.Value} {record.Unit}
                                - **Week of Pregnancy**: {record.Week}
                                - **Description**: {record.Description}
                                - **Additional Notes**: {record.Note}
                                
                                Based on standard fetal growth data, how does this measurement compare to the expected range for week {record.Week}?
                                1. If it is within the normal range, confirm it.
                                2. If it is outside the expected range, categorize the severity as LOW, MODERATE, HIGH, or CRITICAL.
                                3. If no standard data exists for this measurement, estimate a reasonable range and categorize the severity accordingly.
                                4. Provide a **detailed medical recommendation** for the user.
                                5. Output response in JSON format:
                                   ```json
                                   {{
                                      ""severity"": ""(NORMAL | LOW | MODERATE | HIGH | CRITICAL)"",
                                      ""expectedRange"": {{
                                          ""min"": (number),
                                          ""max"": (number)
                                      }},
                                      ""recommendation"": ""(String - medical guidance for this case)""
                                   }}
                                 ";

            var issueHtml = $@"
                                <p>A new fetal growth record has been added with the following details:</p>
                                <ul>
                                    <li><strong>Measurement Name:</strong> {record.Name}</li>
                                    <li><strong>Measurement Value:</strong> {record.Value} {record.Unit}</li>
                                    <li><strong>Week of Pregnancy:</strong> {record.Week}</li>
                                    <li><strong>Description:</strong> {record.Description}</li>
                                    <li><strong>Additional Notes:</strong> {record.Note}</li>
                                </ul>
                                <p>Based on standard fetal growth data, how does this measurement compare to the expected range for week {record.Week}?</p>
                                <ol>
                                    <li>If it is within the normal range, confirm it.</li>
                                    <li>If it is outside the expected range, categorize the severity as <strong>LOW, MODERATE, HIGH, or CRITICAL</strong>.</li>
                                    <li>If no standard data exists for this measurement, estimate a reasonable range and categorize the severity accordingly.</ li>
                                    <li>Provide a <strong>detailed medical recommendation</strong> for the user.</li>
                                    <li>Output response in JSON format:</li>
                                </ol>";

            var recommendation = await _chatGPTService.GenerateRecommendation(issuePrompt);
            if (string.IsNullOrEmpty(recommendation)) return string.Empty;
            var alert = new GrowthAlert
            {
                Id = Guid.NewGuid(),
                FetalGrowthRecordId = record.Id,
                UserId = userId,
                Week = record.Week,
                Issue = issueHtml,
                Severity = severity.ToString(),
                Recommendation = recommendation,
                AlertFor = "Fetal",
                AlertDate = DateTime.Now,
            };

            await _context.GrowthAlerts.AddAsync(alert);
            await _context.SaveChangesAsync();
            return recommendation.FirstOrDefault().ToString() ?? string.Empty;
        }

        public async Task<List<GrowthAlert>> GetGrowthAlerts(Guid userId)
        {
            var growthAlerts = _context.GrowthAlerts.AsNoTracking().Where(x => x.UserId == userId && x.IsDeleted == false).OrderBy(x => x.Week).ThenBy(x => x.IsResolved);

            return await growthAlerts.ToListAsync();
        }

        public async Task<bool> UpdateStatusGrowthAlert(Guid growthAlertId, string status)
        {
            var growthAlert = await _context.GrowthAlerts.FirstOrDefaultAsync(x => x.Id == growthAlertId && x.IsDeleted == false);
            if (growthAlert == null)
            {
                return false;
            }

            growthAlert.IsResolved = true;
            return true;
        }

        private SeverityEnum GetSeverity(double? value, double? min, double? max)
        {
            double range = max - min ?? 0;
            double lowerBound = min - (0.1 * range) ?? 0;
            double upperBound = max + (0.1 * range) ?? 0;

            if (value >= min && value <= max)
                return SeverityEnum.Normal;
            if (value < lowerBound)
                return SeverityEnum.Critical;
            if (value < min)
                return SeverityEnum.High;
            if (value > upperBound)
                return SeverityEnum.Critical;
            if (value > max)
                return SeverityEnum.High;
            return SeverityEnum.Moderate;
        }
    }
}
