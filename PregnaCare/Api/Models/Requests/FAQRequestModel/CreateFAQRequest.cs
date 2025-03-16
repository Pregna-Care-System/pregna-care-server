using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.FAQRequestModel
{
    public class CreateFAQRequest : AbstractApiRequest
    {
        public CreateFAQEntity CreateFAQEntity { get; set; }
    }

    public class CreateFAQEntity
    {
        public Guid FAQCategoryId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int DisplayOrder { get; set; }
    }
}
