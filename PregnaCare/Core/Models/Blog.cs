using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class Blog
{
    public int BlogId { get; set; }

    public int AuthorId { get; set; }

    public string PageTitle { get; set; }

    public string Content { get; set; }

    public string ShortDescription { get; set; }

    public string FeaturedImageUrl { get; set; }

    public string UrlHandle { get; set; }

    public bool IsVisible { get; set; }

    public int? ViewCount { get; set; }

    public DateTime? PublishedDate { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual UserAccount Author { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
