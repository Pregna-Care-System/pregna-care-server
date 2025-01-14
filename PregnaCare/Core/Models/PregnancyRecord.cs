using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class PregnancyRecord
{
    public int Id { get; set; }

    public string BabyName { get; set; }

    public DateOnly PregnancyStartDate { get; set; }

    public DateOnly ExpectedDueDate { get; set; }

    public int CurrentWeek { get; set; }

    public string BabyGender { get; set; }

    public int MotherAge { get; set; }

    public string MotherBloodType { get; set; }

    public double? MotherWeightBeforePregnancy { get; set; }

    public string HealthConditions { get; set; }

    public string ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<FetalGrowthRecord> FetalGrowthRecords { get; set; } = new List<FetalGrowthRecord>();

    public virtual User User { get; set; }
}
