using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.FAQCategoryRequestModel
{
    public class UpdateFAQCategoryRequest : AbstractApiRequest
    {
        public UpdateFAQCategoryEntity UpdateFAQCategoryEntity { get; set; }
    }

    public class UpdateFAQCategoryEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
    }
}
