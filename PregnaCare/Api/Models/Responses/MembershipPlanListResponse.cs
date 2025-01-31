using PregnaCare.Common.Api;
using PregnaCare.Core.Models;

namespace PregnaCare.Api.Models.Responses
{
    public class MembershipPlanListResponse : AbstractApiResponse<IEnumerable<MembershipPlan>>
    {
        public override IEnumerable<MembershipPlan> Response { get; set; }
    }
}
