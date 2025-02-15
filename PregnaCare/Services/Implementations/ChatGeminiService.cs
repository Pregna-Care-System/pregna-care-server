using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using PregnaCare.Api.Models.Responses;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class ChatGeminiService : IChatGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _geminiUrl;

        public ChatGeminiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? string.Empty;
            _geminiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key=";
        }

        public async Task<string> CallChatBotApi(string prompt)
        {
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_geminiUrl + _apiKey, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error API: {response.StatusCode}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<GeminiResponse>(responseJson);

            if (responseObject?.Candidates != null && responseObject.Candidates.Count > 0)
            {
                return Regex.Replace(responseObject.Candidates[0].Content.Parts[0].Text, @"^```json\n|\n```$", "").Trim(); 
            }

            return "No recommendation available.";
        }
    }
}
