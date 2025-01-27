using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class Feature
{
    public Guid Id { get; set; }

    public string FeatureName { get; set; }

    public string Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<MembershipPlanFeature> MembershipPlanFeatures { get; set; } = new List<MembershipPlanFeature>();
}
