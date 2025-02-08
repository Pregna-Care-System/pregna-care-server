using PregnaCare.Common.Api;
using PregnaCare.Core.DTOs;
using PregnaCare.Core.Models;

namespace PregnaCare.Api.Models.Responses
{
    public class MembershipPlanResponse : AbstractApiResponse<MembershipPlanFeatureDTO>
    {
        public override MembershipPlanFeatureDTO Response { get; set; }

    }
}
