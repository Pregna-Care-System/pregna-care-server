using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class BlogTag
{
    public Guid Id { get; set; }

    public Guid BlogId { get; set; }

    public Guid TagId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Blog Blog { get; set; }

    public virtual Tag Tag { get; set; }
}
