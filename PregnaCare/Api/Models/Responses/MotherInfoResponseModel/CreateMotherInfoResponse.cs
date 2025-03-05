using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.MotherInfoResponseModel
{
    public class CreateMotherInfoResponse : AbstractApiResponse<string>
    {
        public override string Response { get; set; }
    }
}
