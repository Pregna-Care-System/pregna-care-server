namespace PregnaCare.Services.Interfaces
{
    public interface IChatGPTService
    {
        Task<string> GenerateRecommendation(string issue);
        Task<(double MinValue, double MaxValue)> GetEstimatedRange(string metricName, int week);
        Task<string> CallChatGptApi(string prompt);
    }
}
