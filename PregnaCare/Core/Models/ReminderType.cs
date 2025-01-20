using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class ReminderType
{
    public int Id { get; set; }

    public string TypeName { get; set; }

    public string Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
}
