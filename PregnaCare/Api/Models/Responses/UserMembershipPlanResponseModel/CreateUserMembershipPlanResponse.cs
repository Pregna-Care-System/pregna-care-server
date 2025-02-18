using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.UserMembershipPlanResponseModel
{
    public class CreateUserMembershipPlanResponse : AbstractApiResponse<string>
    {
        public override string Response { get; set; }
    }
}
