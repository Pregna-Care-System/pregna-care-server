using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class MotherInfo
{
    public int Id { get; set; }

    public int PregnancyRecordId { get; set; }

    public int Week { get; set; }

    public int? HeartRate { get; set; }

    public double? Weight { get; set; }

    public string BloodPressure { get; set; }

    public string HealthStatus { get; set; }

    public string Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual PregnancyRecord PregnancyRecord { get; set; }
}
