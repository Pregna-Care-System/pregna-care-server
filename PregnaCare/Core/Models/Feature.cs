using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class Feature
{
    public int Id { get; set; }

    public string FeatureName { get; set; }

    public string Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<MembershipPlan> MembershipPlans { get; set; } = new List<MembershipPlan>();
}
