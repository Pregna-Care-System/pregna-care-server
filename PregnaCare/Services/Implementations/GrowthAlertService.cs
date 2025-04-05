using System.Text.Json;
using System.Text;
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
                                     $"Expected Range in Week {record.Week}: {min} - {max} ({growthMetric.Unit})\n" +
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

        public async Task<List<GrowthAlert>> ProcessBatchFetalGrowthRecords(Guid userId, List<FetalGrowthRecord> records)
        {
            var alerts = new List<GrowthAlert>();

            // STEP 1: Group records by the ones we can process immediately vs. those needing API calls
            var recordsByPresence = await GroupRecordsByMetricPresence(records);
            var recordsWithMetrics = recordsByPresence.WithMetrics;
            var recordsNeedingApiCall = recordsByPresence.NeedingApiCall;

            // STEP 2: Process records that already have metrics (no API call needed)
            foreach (var (record, metric) in recordsWithMetrics)
            {
                var alert = await CreateGrowthAlertFromExistingMetric(userId, record, metric);
                if (alert != null)
                {
                    alerts.Add(alert);
                }
            }

            // STEP 3: Only make API call if there are records needing it
            if (recordsNeedingApiCall.Any())
            {
                try
                {
                    // Generate batch prompt for all missing metrics
                    var promptText = GenerateBatchPrompt(recordsNeedingApiCall);

                    // Make a single API call instead of multiple ones
                    var batchResponse = await _geminiService.CallChatBotApi(promptText);

                    // Parse the batch response
                    var parsedResults = ParseBatchResponse(batchResponse, recordsNeedingApiCall);

                    // Process each parsed result
                    foreach (var (name, week, value, unit, min, max, severity, issue, recommendation) in parsedResults)
                    {
                        // Find the matching record
                        var record = recordsNeedingApiCall.FirstOrDefault(r =>
                            r.Name == name && r.Week == week);

                        if (record == null) continue;

                        // Create new GrowthMetric and save to database
                        var newMetric = new GrowthMetric
                        {
                            Id = Guid.NewGuid(),
                            Name = name,
                            Week = week,
                            MinValue = min,
                            MaxValue = max,
                            Unit = unit,
                            Description = $"Standard range for {name} at week {week}",
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            IsDeleted = false
                        };

                        await _context.GrowthMetrics.AddAsync(newMetric);

                        // Create alert
                        var fullRecommendation = $"Severity: {severity}\n" +
                                                 $"Expected Range in Week {week}: {min} - {max} {unit}\n" +
                                                 $"Recommendation: {recommendation}";

                        var alert = new GrowthAlert
                        {
                            Id = Guid.NewGuid(),
                            FetalGrowthRecordId = record.Id,
                            UserId = userId,
                            Week = record.Week,
                            Issue = issue,
                            Severity = severity,
                            Recommendation = fullRecommendation,
                            AlertFor = record.Description?.ToLower().Contains("fetal") == true ? "Fetal" : "Mother",
                            AlertDate = DateTime.Now,
                            Status = string.Empty,
                            IsDeleted = false,
                            IsResolved = false,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };

                        await _context.GrowthAlerts.AddAsync(alert);
                        alerts.Add(alert);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in batch processing: {ex.Message}");
                    // Consider adding fallback mechanism here
                }
            }

            return alerts;
        }

        private async Task<(List<(FetalGrowthRecord Record, GrowthMetric Metric)> WithMetrics, List<FetalGrowthRecord> NeedingApiCall)>
            GroupRecordsByMetricPresence(List<FetalGrowthRecord> records)
        {
            var recordsWithMetrics = new List<(FetalGrowthRecord Record, GrowthMetric Metric)>();
            var recordsNeedingApiCall = new List<FetalGrowthRecord>();

            // Get all unique week numbers and metric names
            var weekNumbers = records.Select(r => r.Week).Distinct().ToList();
            var metricNames = records.Select(r => r.Name).Distinct().ToList();

            // Get all existing metrics that match our records in a single query
            var existingMetrics = await _context.GrowthMetrics
                .AsNoTracking()
                .Where(g => weekNumbers.Contains(g.Week) &&
                            metricNames.Contains(g.Name) &&
                            !g.IsDeleted.Value)
                .ToListAsync();

            // Group records based on whether we have metrics for them
            foreach (var record in records)
            {
                var metric = existingMetrics.FirstOrDefault(m =>
                    m.Name == record.Name && m.Week == record.Week);

                if (metric != null)
                {
                    recordsWithMetrics.Add((record, metric));
                }
                else
                {
                    recordsNeedingApiCall.Add(record);
                }
            }

            return (recordsWithMetrics, recordsNeedingApiCall);
        }

        private async Task<GrowthAlert> CreateGrowthAlertFromExistingMetric(Guid userId, FetalGrowthRecord record, GrowthMetric metric)
        {
            var severity = GetSeverity(record.Value, metric.MinValue, metric.MaxValue);

            // Generate recommendation based on severity
            string issue = "";
            string recommendation = "";

            if (severity == SeverityEnum.Normal)
            {
                issue = "No concerns identified";
                recommendation = $"The {record.Name} is within the normal range of {metric.MinValue} - {metric.MaxValue} {record.Unit} for week {record.Week}.";
            }
            else if (severity == SeverityEnum.Low || severity == SeverityEnum.Moderate)
            {
                issue = $"{record.Name} is below the expected range";
                recommendation = $"The {record.Name} is slightly below the normal range of {metric.MinValue} - {metric.MaxValue} {record.Unit} for week {record.Week}. " +
                                "Consider discussing with your healthcare provider during your next appointment.";
            }
            else if (severity == SeverityEnum.High)
            {
                issue = $"{record.Name} is above the expected range";
                recommendation = $"The {record.Name} is above the normal range of {metric.MinValue} - {metric.MaxValue} {record.Unit} for week {record.Week}. " +
                                "This may require attention. Please consult with your healthcare provider.";
            }
            else if (severity == SeverityEnum.Critical)
            {
                issue = $"{record.Name} is significantly outside the expected range";
                recommendation = $"The {record.Name} is significantly outside the normal range of {metric.MinValue} - {metric.MaxValue} {record.Unit} for week {record.Week}. " +
                                "This requires immediate attention. Please contact your healthcare provider as soon as possible.";
            }

            var fullRecommendation = $"Severity: {severity}\n" +
                                     $"Expected Range in Week {record.Week}: {metric.MinValue} - {metric.MaxValue} {record.Unit}\n" +
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
                AlertFor = record.Description?.ToLower().Contains("fetal") == true ? "Fetal" : "Mother",
                AlertDate = DateTime.Now,
                Status = string.Empty,
                IsDeleted = false,
                IsResolved = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _context.GrowthAlerts.AddAsync(alert);
            await _context.SaveChangesAsync();

            return alert;
        }

        private string GenerateBatchPrompt(List<FetalGrowthRecord> records)
        {
            var prompt = new StringBuilder();
            _ = prompt.AppendLine("I need analysis for multiple fetal growth measurements:");
            _ = prompt.AppendLine();

            // Group by week for better context
            var recordsByWeek = records.GroupBy(r => r.Week);

            foreach (var weekGroup in recordsByWeek)
            {
                _ = prompt.AppendLine($"## Week {weekGroup.Key}:");
                foreach (var record in weekGroup)
                {
                    _ = prompt.AppendLine($"- **{record.Name}**: {record.Value} {record.Unit} {(!string.IsNullOrEmpty(record.Description) ? $"({record.Description})" : "")}");
                }
            }

            _ = prompt.AppendLine("\n### Instructions:");
            _ = prompt.AppendLine("For EACH measurement above:");
            _ = prompt.AppendLine("1. Determine the standard range (min and max values) for the specific week");
            _ = prompt.AppendLine("2. Evaluate if the value is NORMAL, LOW, MODERATE, HIGH, or CRITICAL");
            _ = prompt.AppendLine("3. Identify any potential issues");
            _ = prompt.AppendLine("4. Provide medical recommendations");

            _ = prompt.AppendLine("\n### Response Format:");
            _ = prompt.AppendLine("Return a valid JSON array with each measurement analyzed separately:");
            _ = prompt.AppendLine("```json");
            _ = prompt.AppendLine("[");
            _ = prompt.AppendLine("  {");
            _ = prompt.AppendLine("    \"name\": \"Weight\",");
            _ = prompt.AppendLine("    \"week\": 20,");
            _ = prompt.AppendLine("    \"value\": 320,");
            _ = prompt.AppendLine("    \"unit\": \"g\",");
            _ = prompt.AppendLine("    \"expectedRange\": {");
            _ = prompt.AppendLine("      \"min\": 300,");
            _ = prompt.AppendLine("      \"max\": 350");
            _ = prompt.AppendLine("    },");
            _ = prompt.AppendLine("    \"severity\": \"NORMAL\",");
            _ = prompt.AppendLine("    \"issue\": \"No concerns identified\",");
            _ = prompt.AppendLine("    \"recommendation\": \"The weight is within normal range...\"");
            _ = prompt.AppendLine("  },");
            _ = prompt.AppendLine("  {");
            _ = prompt.AppendLine("    \"name\": \"Head Circumference\",");
            _ = prompt.AppendLine("    \"week\": 20,");
            _ = prompt.AppendLine("    \"value\": 18,");
            _ = prompt.AppendLine("    \"unit\": \"cm\",");
            _ = prompt.AppendLine("    \"expectedRange\": {");
            _ = prompt.AppendLine("      \"min\": 16,");
            _ = prompt.AppendLine("      \"max\": 19");
            _ = prompt.AppendLine("    },");
            _ = prompt.AppendLine("    \"severity\": \"NORMAL\",");
            _ = prompt.AppendLine("    \"issue\": \"No concerns identified\",");
            _ = prompt.AppendLine("    \"recommendation\": \"The head circumference is within normal range...\"");
            _ = prompt.AppendLine("  }");
            _ = prompt.AppendLine("]");
            _ = prompt.AppendLine("```");
            _ = prompt.AppendLine("IMPORTANT: Provide analysis for ALL measurements listed above. Use the exact severity levels mentioned (NORMAL, LOW, MODERATE, HIGH, CRITICAL).");

            return prompt.ToString();
        }

        private List<(string Name, int Week, double Value, string Unit, double Min, double Max, string Severity, string Issue, string Recommendation)>
    ParseBatchResponse(string response, List<FetalGrowthRecord> records)
        {
            var results = new List<(string Name, int Week, double Value, string Unit, double Min, double Max, string Severity, string Issue, string Recommendation)>();

            try
            {
                // First, try to extract JSON from the response which might be wrapped in code blocks
                string jsonContent = ExtractJsonFromResponse(response);

                if (string.IsNullOrEmpty(jsonContent))
                {
                    throw new Exception("Failed to extract JSON from response");
                }

                // Try to deserialize the JSON array
                using (var doc = JsonDocument.Parse(jsonContent))
                {
                    // Ensure we have a JSON array
                    if (doc.RootElement.ValueKind != JsonValueKind.Array)
                    {
                        throw new Exception("Expected a JSON array in response");
                    }

                    // Process each element in the array
                    foreach (JsonElement item in doc.RootElement.EnumerateArray())
                    {
                        try
                        {
                            // Safely extract values with error handling
                            if (!item.TryGetProperty("name", out JsonElement nameElement) ||
                                !item.TryGetProperty("week", out JsonElement weekElement) ||
                                !item.TryGetProperty("value", out JsonElement valueElement) ||
                                !item.TryGetProperty("unit", out JsonElement unitElement) ||
                                !item.TryGetProperty("expectedRange", out JsonElement rangeElement) ||
                                !item.TryGetProperty("severity", out JsonElement severityElement) ||
                                !item.TryGetProperty("issue", out JsonElement issueElement) ||
                                !item.TryGetProperty("recommendation", out JsonElement recElement))
                            {
                                throw new Exception("Missing required fields in response item");
                            }

                            string name = nameElement.GetString() ?? "";
                            int week = weekElement.GetInt32();
                            double value = valueElement.GetDouble();
                            string unit = unitElement.GetString() ?? "";

                            // Handle the nested expectedRange object
                            if (!rangeElement.TryGetProperty("min", out JsonElement minElement) ||
                                !rangeElement.TryGetProperty("max", out JsonElement maxElement))
                            {
                                throw new Exception("Missing min/max in expectedRange");
                            }

                            double min = minElement.GetDouble();
                            double max = maxElement.GetDouble();
                            string severity = severityElement.GetString() ?? "UNKNOWN";
                            string issue = issueElement.GetString() ?? "";
                            string recommendation = recElement.GetString() ?? "";

                            results.Add((name, week, value, unit, min, max, severity, issue, recommendation));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error parsing item: {ex.Message}");
                            // Continue to next item even if this one fails
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing batch response: {ex.Message}");

                // Fallback: Create basic entries for each record
                foreach (var record in records)
                {
                    // Use estimated values as fallback
                    double estimatedMin = Math.Max(0, (record.Value ?? 0) * 0.8);
                    double estimatedMax = (record.Value ?? 0) * 1.2;

                    results.Add((
                        record.Name,
                        record.Week ?? 0,
                        record.Value ?? 0,
                        record.Unit,
                        estimatedMin,
                        estimatedMax,
                        "UNKNOWN",
                        "Unable to analyze this measurement properly",
                        "Please consult with your healthcare provider for proper evaluation."
                    ));
                }
            }

            return results;
        }

        private string ExtractJsonFromResponse(string response)
        {
            // Try to extract JSON between code block markers
            var match = Regex.Match(response, @"```(?:json)?\s*(\[[\s\S]*?\])\s*```", RegexOptions.Singleline);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            // If not found, try to find a JSON array directly
            match = Regex.Match(response, @"\[\s*\{\s*""name""\s*:[\s\S]*?\}\s*\]", RegexOptions.Singleline);
            if (match.Success)
            {
                return match.Value;
            }

            // Last resort: try to find anything that looks like a JSON array
            match = Regex.Match(response, @"\[\s*\{[\s\S]*?\}\s*\]", RegexOptions.Singleline);
            if (match.Success)
            {
                return match.Value;
            }

            return string.Empty;
        }

        Task<(List<(FetalGrowthRecord Record, GrowthMetric Metric)> WithMetrics, List<FetalGrowthRecord> NeedingApiCall)> IGrowthAlertService.GroupRecordsByMetricPresence(List<FetalGrowthRecord> records)
        {
            return GroupRecordsByMetricPresence(records);
        }

        Task<GrowthAlert> IGrowthAlertService.CreateGrowthAlertFromExistingMetric(Guid userId, FetalGrowthRecord record, GrowthMetric metric)
        {
            return CreateGrowthAlertFromExistingMetric(userId, record, metric);
        }
    }
}
