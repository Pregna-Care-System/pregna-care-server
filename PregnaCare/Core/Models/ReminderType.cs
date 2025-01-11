using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class ReminderType
{
    public int ReminderTypeId { get; set; }

    public string TypeName { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
}
