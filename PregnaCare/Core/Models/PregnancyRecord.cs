using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class PregnancyRecord
{
    public int PregnancyRecordId { get; set; }

    public int? UserAccountId { get; set; }

    public string BabyName { get; set; }

    public string MotherName { get; set; }

    public string FatherName { get; set; }

    public string ContactPhone { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly DueDate { get; set; }

    public int? WeeksPregnant { get; set; }

    public string PregnancyType { get; set; }

    public DateTime? LastUpdated { get; set; }

    public virtual ICollection<GrowthMetric> GrowthMetrics { get; set; } = new List<GrowthMetric>();

    public virtual UserAccount UserAccount { get; set; }
}
