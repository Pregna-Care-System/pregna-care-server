using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class BlogTag
{
    public int Id { get; set; }

    public int BlogId { get; set; }

    public int TagId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Blog Blog { get; set; }

    public virtual Tag Tag { get; set; }
}
