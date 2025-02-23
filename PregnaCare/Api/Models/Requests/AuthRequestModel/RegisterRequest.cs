using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.AuthRequestModel
{
    public class RegisterRequest : AbstractApiRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
    }
}
