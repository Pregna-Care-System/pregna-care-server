using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.BlogRequestModel
{
    public class BlogRequest : AbstractApiRequest
    {
        public Guid UserId { get; set; }

        public string PageTitle { get; set; } = string.Empty;

        public string Heading { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string ShortDescription { get; set; } = string.Empty;

        public string FeaturedImageUrl { get; set; } = string.Empty;

        public string UrlHandle { get; set; } = string.Empty;

    }
}