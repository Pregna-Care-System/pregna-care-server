namespace PregnaCare.Core.Models;
public partial class UserRole
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public Guid? RoleId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Role Role { get; set; }

    public virtual User User { get; set; }
}