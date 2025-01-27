using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class MembershipPlan
{
    public Guid Id { get; set; }

    public string PlanName { get; set; }

    public double Price { get; set; }

    public int Duration { get; set; }

    public string Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<MembershipPlanFeature> MembershipPlanFeatures { get; set; } = new List<MembershipPlanFeature>();

    public virtual ICollection<UserMembershipPlan> UserMembershipPlans { get; set; } = new List<UserMembershipPlan>();
}
