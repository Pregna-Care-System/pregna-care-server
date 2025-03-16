using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Responses.FAQCategoryResponseModel
{
    public class SelectDetailFAQCategoryResponse : AbstractApiResponse<SelectFAQCategoryEntity>
    {
        public override SelectFAQCategoryEntity Response { get; set; }
    }
}
