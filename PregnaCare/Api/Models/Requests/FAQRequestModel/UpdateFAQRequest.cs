using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.FAQRequestModel
{
    public class UpdateFAQRequest : AbstractApiRequest
    {
        public UpdateFAQEntity UpdateFAQEntity { get; set; }
    }

    public class UpdateFAQEntity
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public int DisplayOrder { get; set; }
    }
}
