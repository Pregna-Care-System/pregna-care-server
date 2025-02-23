using PregnaCare.Api.Models.Responses.StatisticsResponseModel;

namespace PregnaCare.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<StatsResponse> GetMemberStatisticsAsync();
        Task<StatsResponse> GetUserStatisticsAsync();
        Task<StatsResponse> GetTotalTransactionStatisticsAsync();
        Task<StatsResponse> GetTotalRevenueStatisticsAsync();
        Task<List<MembershipStatsResponse>> GetMembershipStatsAsync();
        Task<(int, int, int, List<TransactionStatsResponse>)> GetRecentTransactionsAsync(int offset, int limit);
        Task<List<RevenueStatsResponse>> GetTotalRevenueAsync();
        Task<List<NewMemberResponse>> GetNewMembersAsync(int? year, int? month, int? week);
    }
}
