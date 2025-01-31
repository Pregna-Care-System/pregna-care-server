using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;
using PregnaCare.Core.Models;

namespace PregnaCare.Services.Interfaces
{
    public interface IMembershipPlansService
    {
        Task<MembershipPlanListResponse> GetAllPlansAsync();
        Task<MembershipPlanResponse> GetPlanByIdAsync(Guid id);
        Task<MembershipPlanResponse> AddPlanAsync(MembershipPlanRequest request, List<Guid> featureIds);
        Task UpdatePlanAsync(Guid id, MembershipPlan plan);
        Task<MembershipPlanResponse> DeletePlanAsync(Guid id);

    }
}
