using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class FetalStandard
{
    public int Id { get; set; }

    public int Week { get; set; }

    public int GrowthMetricId { get; set; }

    public double MinValue { get; set; }

    public double MaxValue { get; set; }

    public string Descriptioon { get; set; }

    public string Source { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual GrowthMetric GrowthMetric { get; set; }
}
