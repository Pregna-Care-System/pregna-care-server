using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int? BlogId { get; set; }

    public int? UserAccountId { get; set; }

    public string CommentText { get; set; }

    public DateTime? CommentDate { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Blog Blog { get; set; }

    public virtual UserAccount UserAccount { get; set; }
}
