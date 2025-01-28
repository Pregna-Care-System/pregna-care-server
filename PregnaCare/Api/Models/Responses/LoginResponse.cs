using PregnaCare.Common.Api;
using PregnaCare.Core.Models;

namespace PregnaCare.Api.Models.Responses
{
    public class LoginResponse : AbstractApiResponse<Token>
    {
        public override Token Response { get; set; }
    }
}
