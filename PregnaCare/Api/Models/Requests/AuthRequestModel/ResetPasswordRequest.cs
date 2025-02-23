using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.AuthRequestModel
{
    public class ResetPasswordRequest : AbstractApiRequest
    {
        public string Token { get; set; }

        public string NewPassword { get; set; }
    }
}
