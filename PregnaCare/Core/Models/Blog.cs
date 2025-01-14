using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class Blog
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string PageTitle { get; set; }

    public string Heading { get; set; }

    public string Content { get; set; }

    public string ShortDescription { get; set; }

    public string FeaturedImageUrl { get; set; }

    public string UrlHandle { get; set; }

    public bool? IsVisible { get; set; }

    public int? ViewCount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual User User { get; set; }
}
