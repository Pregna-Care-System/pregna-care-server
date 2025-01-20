using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class Notification
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ReminderId { get; set; }

    public string Title { get; set; }

    public string Message { get; set; }

    public string Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Reminder Reminder { get; set; }

    public virtual User User { get; set; }
}
