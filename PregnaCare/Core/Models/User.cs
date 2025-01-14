using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class User
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public string Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string Address { get; set; }

    public string ImageUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<GrowthAlert> GrowthAlerts { get; set; } = new List<GrowthAlert>();

    public virtual ICollection<PregnancyRecord> PregnancyRecords { get; set; } = new List<PregnancyRecord>();

    public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
}
