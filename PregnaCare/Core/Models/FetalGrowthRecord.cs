using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class FetalGrowthRecord
{
    public Guid Id { get; set; }

    public Guid PregnancyRecordId { get; set; }

    public Guid GrowthMetricId { get; set; }

    public int? Week { get; set; }

    public double? Value { get; set; }

    public string Note { get; set; } = string.Empty; 

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<GrowthAlert> GrowthAlerts { get; set; } = new List<GrowthAlert>();

    public virtual GrowthMetric GrowthMetric { get; set; }

    public virtual PregnancyRecord PregnancyRecord { get; set; }
}
