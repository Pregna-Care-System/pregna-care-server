using System.Text.Json;
using System.Text;
using PregnaCare.Services.Interfaces;
using System.Text.RegularExpressions;
using PregnaCare.Api.Models.Responses;
using System.Net.Http.Headers;

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

        public async Task<string> CallChatGptApi(string prompt)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable("CHATGPT_API_KEY"));

                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[] { new { role = "user", content = prompt } }    
                };

                var response = await httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestBody);
                var responseJson = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<ChatGPTResponse>(responseJson);

                return responseObject?.Choices?.FirstOrDefault()?.Message?.Content ?? "No recommendation available.";
            }
        }

        public async Task<string> GenerateRecommendation(string issue)
        {
            string prompt = $"A user has an issue: {issue}. Provide a detailed medical recommendation for them.";

            return await CallChatGptApi(prompt);
        }

        public async Task<(double MinValue, double MaxValue)> GetEstimatedRange(string metricName, int week)
        {
            string prompt = $"Estimate a reasonable growth range for '{metricName}' in week {week}. Provide a minimum and maximum value.";

            string response = await CallChatGptApi(prompt);

            var match = Regex.Match(response, @"Min:\s*(\d+\.?\d*),\s*Max:\s*(\d+\.?\d*)");
            if (match.Success)
            {
                double minValue = double.Parse(match.Groups[1].Value);
                double maxValue = double.Parse(match.Groups[2].Value);
                return (minValue, maxValue);
            }

            return (0.0, 0.0);
        }
    }
}
