using PregnaCare.Api.Models.Responses.StatisticsResponseModel;

namespace PregnaCare.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<StatsResponse> GetMemberStatisticsAsync();
        Task<StatsResponse> GetUserStatisticsAsync();
        Task<StatsResponse> GetTotalTransactionStatisticsAsync();
        Task<StatsResponse> GetTotalRevenueStatisticsAsync();
    }
}
