using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.FAQResponseModel
{
    public class UpdateFAQResponse : AbstractApiResponse<string>
    {
        public override string Response { get; set; }
    }
}
