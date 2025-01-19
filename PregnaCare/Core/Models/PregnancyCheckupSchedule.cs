using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class PregnancyCheckupSchedule
{
    public int Id { get; set; }

    public int Week { get; set; }

    public string Description { get; set; } = string.Empty;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }
}
