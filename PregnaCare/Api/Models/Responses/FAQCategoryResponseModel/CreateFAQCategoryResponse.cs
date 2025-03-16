using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.FAQCategoryResponseModel
{
    public class CreateFAQCategoryResponse : AbstractApiResponse<string>
    {
        public override string Response { get; set; }
    }
}
