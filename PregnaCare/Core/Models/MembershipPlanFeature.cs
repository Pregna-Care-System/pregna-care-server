using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PregnaCare.Core.Models;

public partial class MembershipPlanFeature
{
    public Guid Id { get; set; }

    public Guid MembershipPlanId { get; set; }

    public Guid FeatureId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }
    [JsonIgnore]
    public virtual Feature Feature { get; set; }
    [JsonIgnore]
    public virtual MembershipPlan MembershipPlan { get; set; }
}
