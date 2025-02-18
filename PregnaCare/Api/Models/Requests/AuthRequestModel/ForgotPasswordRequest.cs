using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.AuthRequestModel
{
    public class ForgotPasswordRequest : AbstractApiRequest
    {
        public string Email { get; set; }
    }
}
