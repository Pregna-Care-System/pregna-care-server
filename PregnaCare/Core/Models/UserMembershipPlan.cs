using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class UserMembershipPlan
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int MembershipPlanId { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual MembershipPlan MembershipPlan { get; set; }

    public virtual User User { get; set; }
}
