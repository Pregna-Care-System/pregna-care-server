using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using PregnaCare.Api.Models.Responses;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class ChatGPTService : IChatGPTService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public ChatGPTService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = Environment.GetEnvironmentVariable("CHATGPT_API_KEY") ?? string.Empty;
        }

        public async Task<string> CallChatBotApi(string prompt)
        {
            using (_httpClient)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable(_apiKey));

                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[] { new { role = "user", content = prompt } }
                };

                var response = await _httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestBody);
                var responseJson = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<ChatGPTResponse>(responseJson);

                return responseObject?.Choices?.FirstOrDefault()?.Message?.Content ?? "No recommendation available.";
            }
        }

        public async Task<string> GenerateRecommendation(IChatBotService chatBotService, string issue)
        {
            string prompt = $"A user has an issue: {issue}. Provide a detailed medical recommendation for them.";

            return await chatBotService.CallChatBotApi(prompt);
        }

        public async Task<(double MinValue, double MaxValue)> GetEstimatedRange(IChatBotService chatBotService, string metricName, int week)
        {
            var prompt = $@"Estimate the reasonable growth range for '{metricName}' in week {week}. 
                            Respond strictly in the following JSON format without any extra text:
                            
                            {{
                              ""minimum"": {{ ""value"": 0.0, ""unit"": ""unit"" }},
                              ""maximum"": {{ ""value"": 0.0, ""unit"": ""unit"" }}
                            }}";


            var response = await chatBotService.CallChatBotApi(prompt);
            var regex = new Regex(@"\{\s*""minimum""\s*:\s*\{\s*""value""\s*:\s*([\d.]+)\s*,\s*""unit""\s*:\s*""([^""]+)""\s*\}\s*,\s*""maximum""\s*:\s*\{\s*""value""\s*:\s*([\d.]+)\s*,\s*""unit""\s*:\s*""([^""]+)""\s*\}\s*\}", RegexOptions.Singleline);

            var match = regex.Match(response);
            if (match != null && match.Success)
            {
                double minValue = double.Parse(match.Groups[1].Value);
                double maxValue = double.Parse(match.Groups[3].Value);
                return (minValue, maxValue);
            }

            return (0.0, 0.0);
        }
    }
}
