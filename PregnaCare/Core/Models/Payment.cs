using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int UserAccountId { get; set; }

    public DateTime PaymentDateTime { get; set; }

    public double Amount { get; set; }

    public string PaymentMethod { get; set; }

    public string PaymentStatus { get; set; }

    public virtual ICollection<MembershipPlanHistory> MembershipPlanHistories { get; set; } = new List<MembershipPlanHistory>();

    public virtual UserAccount UserAccount { get; set; }
}
