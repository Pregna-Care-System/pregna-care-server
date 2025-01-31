using PregnaCare.Common.Api;
using PregnaCare.Core.Models;

namespace PregnaCare.Api.Models.Responses
{
    public class MembershipPlanResponse : AbstractApiResponse<MembershipPlan>
    {
        public override MembershipPlan Response { get; set; }

    }
}
