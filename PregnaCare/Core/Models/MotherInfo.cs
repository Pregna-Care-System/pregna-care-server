using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class MotherInfo
{
    public Guid Id { get; set; }

    public Guid? PregnancyRecordId { get; set; }

    public int Week { get; set; }

    public int? HeartRate { get; set; }

    public double? Weight { get; set; }

    public string BloodPressure { get; set; } = string.Empty;

    public string HealthStatus { get; set; } = string.Empty;

    public string Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual PregnancyRecord PregnancyRecord { get; set; }
}
