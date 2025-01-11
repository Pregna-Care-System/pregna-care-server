using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class MembershipPlan
{
    public int MembershipPlanId { get; set; }

    public string PlanName { get; set; }

    public double? Price { get; set; }

    public int? Duration { get; set; }

    public string Description { get; set; }

    public virtual ICollection<MembershipPlanHistory> MembershipPlanHistories { get; set; } = new List<MembershipPlanHistory>();
}
