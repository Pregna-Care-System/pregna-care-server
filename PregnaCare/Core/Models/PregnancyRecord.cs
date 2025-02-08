namespace PregnaCare.Core.Models;
public partial class PregnancyRecord
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public string BabyName { get; set; } = string.Empty;

    public DateOnly? PregnancyStartDate { get; set; }

    public DateOnly? ExpectedDueDate { get; set; }

    public string BabyGender { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<FetalGrowthRecord> FetalGrowthRecords { get; set; } = new List<FetalGrowthRecord>();

    public virtual MotherInfo MotherInfo { get; set; }

    public virtual User User { get; set; }
}
