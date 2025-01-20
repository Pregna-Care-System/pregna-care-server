using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class GrowthMetric
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Unit { get; set; }

    public string Description { get; set; }

    public double? MinValue { get; set; }

    public double? MaxValue { get; set; }

    public int? Week { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<FetalGrowthRecord> FetalGrowthRecords { get; set; } = new List<FetalGrowthRecord>();
}
