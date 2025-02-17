namespace PregnaCare.Services.Interfaces
{
    public interface IChatGPTService : IChatBotService
    {
        Task<string> GenerateRecommendation(IChatBotService chatBotService, string issue);
        Task<(double MinValue, double MaxValue)> GetEstimatedRange(IChatBotService chatBotService, string metricName, int week);
    }
}
