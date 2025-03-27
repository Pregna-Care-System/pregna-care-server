namespace PregnaCare.Core.Models;
public partial class MotherInfo
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public string BloodType { get; set; } = string.Empty;

    public string HealthStatus { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User? User { get; set; }

    public virtual ICollection<PregnancyRecord> PregnancyRecords { get; set; } = new List<PregnancyRecord>();
}
