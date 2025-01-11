using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class GrowthAlert
{
    public int AlertId { get; set; }

    public int GrowthMetricId { get; set; }

    public int UserAccountId { get; set; }

    public int Week { get; set; }

    public DateOnly AlertDate { get; set; }

    public string Issue { get; set; }

    public string Severity { get; set; }

    public string Recommendation { get; set; }

    public bool? IsResolved { get; set; }

    public string CreatedBy { get; set; }

    public virtual ICollection<AlertAction> AlertActions { get; set; } = new List<AlertAction>();

    public virtual GrowthMetric GrowthMetric { get; set; }

    public virtual UserAccount UserAccount { get; set; }
}
