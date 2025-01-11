using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class UserAccount
{
    public int UserAccountId { get; set; }

    public string UserName { get; set; }

    public string FullName { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string ModifiedBy { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<GrowthAlert> GrowthAlerts { get; set; } = new List<GrowthAlert>();

    public virtual ICollection<MembershipPlanHistory> MembershipPlanHistories { get; set; } = new List<MembershipPlanHistory>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<PregnancyRecord> PregnancyRecords { get; set; } = new List<PregnancyRecord>();

    public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
}
