using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.FAQResponseModel
{
    public class DeleteFAQResponse : AbstractApiResponse<string>
    {
        public override string Response { get; set; }
    }
}
