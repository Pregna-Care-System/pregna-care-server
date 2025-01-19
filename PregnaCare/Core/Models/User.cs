using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class User
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string PhoneNumber { get; set; }

    public string Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string Address { get; set; }

    public string ImageUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? RoleId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<GrowthAlert> GrowthAlerts { get; set; } = new List<GrowthAlert>();

    public virtual ICollection<JwtToken> JwtTokens { get; set; } = new List<JwtToken>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<PregnancyRecord> PregnancyRecords { get; set; } = new List<PregnancyRecord>();

    public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();

    public virtual Role Role { get; set; }

    public virtual ICollection<UserFeature> UserFeatures { get; set; } = new List<UserFeature>();

    public virtual ICollection<UserOauth> UserOauths { get; set; } = new List<UserOauth>();
}
