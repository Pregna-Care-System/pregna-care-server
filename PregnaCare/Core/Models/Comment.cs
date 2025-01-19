using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class Comment
{
    public int Id { get; set; }

    public int BlogId { get; set; }

    public int UserId { get; set; }

    public int? ParentCommentId { get; set; }

    public string CommentText { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Blog Blog { get; set; }

    public virtual ICollection<Comment> InverseParentComment { get; set; } = new List<Comment>();

    public virtual Comment ParentComment { get; set; }

    public virtual User User { get; set; }
}
