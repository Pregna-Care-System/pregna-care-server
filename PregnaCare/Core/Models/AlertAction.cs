using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class AlertAction
{
    public int AlertActionId { get; set; }

    public int AlertId { get; set; }

    public DateOnly ActionDate { get; set; }

    public string PerformedBy { get; set; }

    public string ActionType { get; set; }

    public string ActionDetail { get; set; }

    public virtual GrowthAlert Alert { get; set; }
}
