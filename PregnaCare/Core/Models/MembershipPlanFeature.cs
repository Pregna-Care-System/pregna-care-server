namespace PregnaCare.Core.Models;
public partial class MembershipPlanFeature
{
    public Guid Id { get; set; }

    public Guid MembershipPlanId { get; set; }

    public Guid FeatureId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Feature Feature { get; set; }

    public virtual MembershipPlan MembershipPlan { get; set; }
}
