using System.Text.Json.Serialization;
using PregnaCare.Common.Enums;

namespace PregnaCare.Core.Models;

public partial class Blog
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string PageTitle { get; set; } = string.Empty;

    public string Heading { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string ShortDescription { get; set; } = string.Empty;

    public string FeaturedImageUrl { get; set; } = string.Empty;

    public string UrlHandle { get; set; } = string.Empty;

    public bool? IsVisible { get; set; }

    public int? ViewCount { get; set; }

    public string? SharedChartData { get; set; }

    public string Type { get; set; } = BlogTypeEnum.Blog.ToString();

    public string Status { get; set; } = StatusEnum.Pending.ToString();

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    [JsonIgnore]
    public virtual ICollection<BlogTag> BlogTags { get; set; } = new List<BlogTag>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual User User { get; set; }
}
