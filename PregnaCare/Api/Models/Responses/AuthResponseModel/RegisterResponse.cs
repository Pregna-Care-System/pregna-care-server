using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.AuthResponseModel
{
    public class RegisterResponse : AbstractApiResponse<string>
    {
        public override string Response { get; set; }
    }
}
