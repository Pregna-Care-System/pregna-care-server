using PregnaCare.Common.Api;

namespace PregnaCare.Api.Controllers.Auth
{
    public class LoginRequest : AbstractApiRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }    
    }
}
