using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests
{
    public class LoginRequest : AbstractApiRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
