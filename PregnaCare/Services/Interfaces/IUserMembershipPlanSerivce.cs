using PregnaCare.Api.Models.Requests.UserMembersipPlanRequestModel;
using PregnaCare.Api.Models.Responses.UserMembershipPlanResponseModel;

namespace PregnaCare.Services.Interfaces
{
    public interface IUserMembershipPlanSerivce
    {
        Task<CreateUserMembershipPlanResponse> ActivateUserMembershipPlan(CreateUserMembershipPlanRequest request);
        Task<UserMembershipPlanListResponse> GetUserMembershipPlanList();
        Task<UserMembershipPlanListResponse> GetUserTransaction(Guid userId);
        Task<UserMembershipPlanListResponse> GetExpiringUserMembershipPlans();

    }
}
