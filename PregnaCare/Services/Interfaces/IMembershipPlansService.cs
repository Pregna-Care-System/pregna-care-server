using PregnaCare.Api.Models.Requests.UserMembersipPlanRequestModel;
using PregnaCare.Api.Models.Responses.UserMembershipPlanResponseModel;

namespace PregnaCare.Services.Interfaces
{
    public interface IMembershipPlansService
    {
        Task<MembershipPlanListResponse> GetPlanWithFeatureAsync();
        Task<MembershipPlanResponse> GetPlanByIdAsync(Guid id);
        Task<MembershipPlanResponse> GetPlanByNameAsync(string name);
        Task<MembershipPlanResponse> AddPlanAsync(MembershipPlanRequest request, List<Guid> featureIds);
        Task<MembershipPlanResponse> UpdatePlanAsync(Guid id, MembershipPlanRequest plan, List<Guid> featureIds);
        Task<MembershipPlanResponse> DeletePlanAsync(Guid id);
        Task<string> GetMostUsedPlanNameAsync();
        Task UpgradeGuestToMemberWithFreePlanAsync(Guid userId);
    }
}
