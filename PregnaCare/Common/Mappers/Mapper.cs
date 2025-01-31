using PregnaCare.Api.Models.Requests;
using PregnaCare.Core.Models;

namespace PregnaCare.Common.Mappers
{
    public static class Mapper
    {
        public static MembershipPlan MapToMembershipPlan(MembershipPlanRequest request)
        {
            return new MembershipPlan
            {
                PlanName = request.PlanName,
                Price = request.Price,
                Duration = request.Duration,
                Description = request.Description
            };
        }
    }
}
