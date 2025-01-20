﻿using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class FetalGrowthRecord
{
    public int Id { get; set; }

    public int PregnancyRecordId { get; set; }

    public int GrowthMetricId { get; set; }

    public int? Week { get; set; }

    public double? Value { get; set; }

    public string Note { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<GrowthAlert> GrowthAlerts { get; set; } = new List<GrowthAlert>();

    public virtual GrowthMetric GrowthMetric { get; set; }

    public virtual PregnancyRecord PregnancyRecord { get; set; }
}
