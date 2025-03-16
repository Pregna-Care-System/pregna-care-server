using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.FAQCategoryResponseModel
{
    public class DeleteFAQCategoryResponse : AbstractApiResponse<string>
    {
        public override string Response { get; set; }
    }
}
