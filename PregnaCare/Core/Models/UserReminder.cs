namespace PregnaCare.Core.Models;
public partial class UserReminder
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid ReminderId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Reminder Reminder { get; set; }

    public virtual User User { get; set; }
}
