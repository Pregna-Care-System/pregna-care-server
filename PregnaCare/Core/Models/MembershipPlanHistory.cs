using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class MembershipPlanHistory
{
    public int PlanHistoryId { get; set; }

    public int? UserAccountId { get; set; }

    public int? PlanId { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public int? PaymentId { get; set; }

    public string Note { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    public virtual Payment Payment { get; set; }

    public virtual MembershipPlan Plan { get; set; }

    public virtual UserAccount UserAccount { get; set; }
}
