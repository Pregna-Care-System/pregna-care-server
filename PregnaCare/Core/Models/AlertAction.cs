using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class AlertAction
{
    public int Id { get; set; }

    public int? GrowthAlertId { get; set; }

    public DateTime? ActionDate { get; set; }

    public string PerformedBy { get; set; }

    public string ActionType { get; set; }

    public string ActionDetail { get; set; }

    public virtual GrowthAlert GrowthAlert { get; set; }
}
