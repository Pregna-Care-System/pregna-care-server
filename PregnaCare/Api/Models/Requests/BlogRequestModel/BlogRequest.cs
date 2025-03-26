using PregnaCare.Common.Api;
using PregnaCare.Common.Enums;

namespace PregnaCare.Api.Models.Requests.BlogRequestModel
{
    public class BlogRequest : AbstractApiRequest
    {
        public Guid UserId { get; set; }

        public List<Guid> TagIds { get; set; }
        public string PageTitle { get; set; } = string.Empty;

        public string Heading { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string ShortDescription { get; set; } = string.Empty;

        public string FeaturedImageUrl { get; set; } = string.Empty;
        public bool IsVisible { get; set; }
        public string Type { get; set; } = BlogTypeEnum.Blog.ToString();
        public string Status { get; set; } = StatusEnum.Pending.ToString();
        public string? SharedChartData { get; set; } = string.Empty;
    }
}