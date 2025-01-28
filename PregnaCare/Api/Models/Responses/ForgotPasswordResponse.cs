using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses
{
    public class ForgotPasswordResponse : AbstractApiResponse<string>
    {
        public override string Response { get; set; }
    }
}
