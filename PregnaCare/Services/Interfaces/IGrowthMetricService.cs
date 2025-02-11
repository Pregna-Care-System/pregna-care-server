using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;
using PregnaCare.Core.Models;

namespace PregnaCare.Services.Interfaces
{
    public interface IGrowthMetricService
    {
        Task<List<GrowthMetric>> GetAllGrowthMetrics();
        Task<GrowthMetric> GetGrowthMetricById(Guid id);
        Task<CreateGrowthMetricResponse> CreateGrowthMetric(CreateGrowthMetricRequest request);
        Task<UpdateGrowthMetricResponse> UpdateGrowthMetric(UpdateGrowthMetricRequest request);
        Task<bool> DeleteGrowthMetric(Guid id);
    }
}
