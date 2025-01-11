using PregnaCare.Common.Api;

namespace PregnaCare.Api.Controllers.Auth
{
    public class RegisterRequest : AbstractApiRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; } 
        public string Password { get; set; }
        public string RoleName { get; set; }
    }
}
