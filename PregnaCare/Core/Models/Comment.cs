﻿namespace PregnaCare.Core.Models;

public partial class Comment
{
    public Guid Id { get; set; }

    public Guid BlogId { get; set; }

    public Guid UserId { get; set; }

    public Guid? ParentCommentId { get; set; }

    public string CommentText { get; set; } = string.Empty;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Blog Blog { get; set; }

    public virtual ICollection<Comment> InverseParentComment { get; set; } = new List<Comment>();

    public virtual Comment ParentComment { get; set; }

    public virtual User User { get; set; }
}
