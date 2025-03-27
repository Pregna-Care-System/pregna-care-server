using System.Text.RegularExpressions;
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
        private readonly IChatGeminiService _geminiService;

        public GrowthAlertService(PregnaCareAppDbContext context, IChatGPTService chatGPTService, IChatGeminiService geminiService)
        {
            _context = context;
            _chatGPTService = chatGPTService;
            _geminiService = geminiService;
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
                var estimatedRange = await _chatGPTService.GetEstimatedRange(_geminiService, record.Name, record.Week ?? 0);
                growthMetric = new GrowthMetric
                {
                    Name = record.Name,
                    Week = record.Week,
                    MinValue = estimatedRange.MinValue,
                    MaxValue = estimatedRange.MaxValue
                };
            }

            var severity = GetSeverity(record.Value, growthMetric.MinValue, growthMetric.MaxValue);

            var issuePrompt = $@"
                               A new fetal growth record has been added with the following detail:
                               - **Measurement Name**: {record.Name}
                               - **Measurement Value**: {record.Value} {record.Unit}
                               - **Week of Pregnancy**: {record.Week}
                               - **Description**: {record.Description}
                               - **Additional Notes**: {record.Note}
                               
                               ### **Analysis:**
                               1. Compare this measurement with the expected growth range for week {record.Week}.
                               2. If it falls within the normal range, confirm it.
                               3. If it is outside the expected range:
                                  - Identify the **issue** causing this deviation.
                                  - Categorize the severity as **LOW, MODERATE, HIGH, or CRITICL**.
                               4. If no standard data exists, estimate a reasonable range and det  ermin    the     severity.
                               5. Provide a **detailed medical recommendation** to guide the user.
                               
                               ### **Output Format (JSON):**
                               ```json
                               {{
                                 ""issue"": ""(String - Explanation of the issue, if any)"",
                                 ""severity"": ""(NORMAL | LOW | MODERATE | HIGH | CRITICAL)"",
                                 ""expectedRange"": {{
                                     ""min"": (number),
                                     ""max"": (number)
                                 }},
                                 ""recommendation"": ""(String - medical guidance for this case)""
                               }}";


            (string issue, string returnSeverity, double min, double max, string recommendation) =
                ParseChatbotResponse(await _chatGPTService.GenerateRecommendation(_geminiService, issuePrompt));

            var fullRecommendation = $"Severity: {returnSeverity}\n" +
                                     $"Expected Range in Week {record.Week}: {min} - {max} ({record.Unit})\n" +
                                     $"Recommendation: {recommendation}";

            var alert = new GrowthAlert
            {
                Id = Guid.NewGuid(),
                FetalGrowthRecordId = record.Id,
                UserId = userId,
                Week = record.Week,
                Issue = issue,
                Severity = severity.ToString(),
                Recommendation = fullRecommendation,
                AlertFor = record.Description.ToLower().Contains("fetal") ? "Fetal" : "Mother",
                AlertDate = DateTime.Now,
                Status = string.Empty,
            };

            _ = await _context.GrowthAlerts.AddAsync(alert);
            _ = await _context.SaveChangesAsync();

            return fullRecommendation.FirstOrDefault().ToString() ?? string.Empty;
        }

        public async Task<List<GrowthAlert>> GetGrowthAlerts(Guid userId)
        {
            var growthAlerts = _context.GrowthAlerts.AsNoTracking().Where(x => x.UserId == userId && x.IsDeleted == false).OrderBy(x => x.Week).ThenBy(x => x.IsResolved);

            return await growthAlerts.ToListAsync();
        }

        public async Task<List<GrowthAlert>> GetGrowthAlertsByPregnancyRecordId(Guid PregnancyRecordId)
        {
            return await _context.GrowthAlerts.AsNoTracking().Include(x => x.FetalGrowthRecord).Where(x => x.FetalGrowthRecord.PregnancyRecordId == PregnancyRecordId && x.IsDeleted == false).OrderBy(x => x.Week).ThenBy(x => x.IsResolved).ToListAsync();
        }

        public async Task<bool> UpdateStatusGrowthAlert(Guid growthAlertId, string status)
        {
            var growthAlert = await _context.GrowthAlerts.FirstOrDefaultAsync(x => x.Id == growthAlertId && x.IsDeleted == false);
            if (growthAlert == null)
            {
                return false;
            }

            growthAlert.IsResolved = true;
            growthAlert.Status = status;

            _ = _context.GrowthAlerts.Update(growthAlert);
            _ = await _context.SaveChangesAsync();

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

        private (string Issue, string Severity, double Min, double Max, string Recommendation) ParseChatbotResponse(string response)
        {
            var regex = new Regex(@"""issue""\s*:\s*""([^""]+)""\s*,\s*""severity""\s*:\s*""([^""]+)""\s*,\s*""expectedRange""\s*:\s*\{\s*""min""\s*:\s*([\d.]+)\s*,\s*""max""\s*:\s*([\d.]+)\s*\}\s*,\s*""recommendation""\s*:\s*""([^""]+)""", RegexOptions.Singleline);

            var match = regex.Match(response);
            if (match.Success)
            {
                var issue = match.Groups[1].Value;
                var severity = match.Groups[2].Value;
                var minValue = double.Parse(match.Groups[3].Value);
                var maxValue = double.Parse(match.Groups[4].Value);
                var recommendation = match.Groups[5].Value;
                return (issue, severity, minValue, maxValue, recommendation);
            }

            return ("Unknown issue", "UNKNOWN", 0.0, 0.0, "No valid recommendation found.");
        }

        public async Task<List<GrowthAlert>> GetFetalGrowthRecordsToSendNotification()
        {
            var notifications = _context.Notifications.AsNoTracking();
            var responseList = _context.GrowthAlerts
                .Include(x => x.FetalGrowthRecord)
                .ThenInclude(x => x.PregnancyRecord)
                .ThenInclude(p => p.MotherInfo)
                .ThenInclude(m => m.User)
                .AsNoTracking()
                .Where(x => !notifications.Any(y => y.SenderId == x.Id && y.ReceiverId == x.FetalGrowthRecord.PregnancyRecord.MotherInfo.UserId));

            return await responseList.ToListAsync();
        }
    }
}
