using PregnaCare.Common.Api;

namespace PregnaCare.Api.Controllers.Auth
{
    public class RegisterResponse : AbstractApiResponse<string>
    {
        public override string Response { get; set; }
    }
}
