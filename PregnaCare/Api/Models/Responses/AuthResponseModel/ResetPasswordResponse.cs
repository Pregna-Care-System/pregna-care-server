using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.AuthResponseModel
{
    public class ResetPasswordResponse : AbstractApiResponse<string>
    {
        public override string Response { get; set; }
    }
}
