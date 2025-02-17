using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;

namespace PregnaCare.Services.Interfaces
{
    public interface IUserMembershipPlanSerivce
    {
        Task<CreateUserMembershipPlanResponse> ActivateUserMembershipPlan(CreateUserMembershipPlanRequest request);
        Task<UserMembershipPlanListResponse> GetUserMembershipPlanList();
    }
}
