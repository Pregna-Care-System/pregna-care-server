using PregnaCare.Common.Api;
using PregnaCare.Core.Models;

namespace PregnaCare.Api.Models.Responses.AuthResponseModel
{
    public class LoginResponse : AbstractApiResponse<Token>
    {
        public override Token Response { get; set; }
    }
}
