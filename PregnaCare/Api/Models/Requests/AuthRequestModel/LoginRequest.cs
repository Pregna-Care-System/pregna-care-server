using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.AuthRequestModel
{
    public class LoginRequest : AbstractApiRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
