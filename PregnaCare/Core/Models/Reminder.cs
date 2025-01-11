using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class Reminder
{
    public int ReminderId { get; set; }

    public int? UserAccountId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime ReminderDate { get; set; }

    public int ReminderTypeId { get; set; }

    public string Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string Note { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ReminderType ReminderType { get; set; }

    public virtual UserAccount UserAccount { get; set; }
}
