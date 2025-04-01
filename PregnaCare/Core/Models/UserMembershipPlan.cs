using PregnaCare.Common.Enums;

namespace PregnaCare.Core.Models;
public partial class UserMembershipPlan
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid MembershipPlanId { get; set; }

    public DateTime? ActivatedAt { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public decimal Price { get; set; }

    public string Status { get; set; } = StatusEnum.InProgress.ToString();

    public DateTime StatusChangedAt { get; set; } = DateTime.Now;

    public string StatusNotes { get; set; } = string.Empty;
    
    public string PaymentErrorCode { get; set; } = string.Empty;
    
    public string PaymentReference { get; set; } = string.Empty;

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual MembershipPlan MembershipPlan { get; set; }

    public virtual User User { get; set; }
}