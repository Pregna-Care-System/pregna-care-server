using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class MembershipPlanFeature
{
    public int Id { get; set; }

    public int MembershipPlanId { get; set; }

    public int FeatureId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Feature Feature { get; set; }

    public virtual MembershipPlan MembershipPlan { get; set; }
}
