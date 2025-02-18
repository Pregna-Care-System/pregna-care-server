using PregnaCare.Common.Api;
using PregnaCare.Core.DTOs;

namespace PregnaCare.Api.Models.Responses.UserMembershipPlanResponseModel
{
    public class UserMembershipPlanListResponse : AbstractApiResponse<IEnumerable<UserMembershipPlanDTO>>
    {
        public override IEnumerable<UserMembershipPlanDTO> Response { get; set; }
    }
}
