using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.MotherInfoResponseModel
{
    public class UpdateMotherInfoResponse : AbstractApiResponse<string>
    {
        public override string Response { get; set; }
    }
}
