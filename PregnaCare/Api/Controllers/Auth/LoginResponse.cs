using PregnaCare.Common.Api;
using PregnaCare.Core.Models;

namespace PregnaCare.Api.Controllers.Auth
{
    public class LoginResponse : AbstractApiResponse<Token>
    {
        public override Token Response { get; set; }
    }
}
