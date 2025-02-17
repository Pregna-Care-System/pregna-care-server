namespace PregnaCare.Core.Models;
public partial class Reminder
{
    public Guid Id { get; set; }

    public Guid ReminderTypeId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime? ReminderDate { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public string Status { get; set; } = string.Empty;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ReminderType ReminderType { get; set; }

    public virtual ICollection<UserReminder> UserReminders { get; set; } = new List<UserReminder>();
}
