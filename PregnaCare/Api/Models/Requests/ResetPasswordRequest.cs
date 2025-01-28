using System.ComponentModel.DataAnnotations;
using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests
{
    public class ResetPasswordRequest : AbstractApiRequest
    {
        public string Email { get; set; }

        public string Token { get; set; }

        public string NewPassword { get; set; }
    }
}
