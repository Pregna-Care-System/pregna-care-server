using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.FAQCategoryRequestModel
{
    public class CreateFAQCategoryRequest : AbstractApiRequest
    {
        public CreateFAQCategoryEntity CreateFAQCategoryEntity { get; set; }
    }

    public class CreateFAQCategoryEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
    }
}
