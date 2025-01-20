using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class GrowthAlert
{
    public int Id { get; set; }

    public int FetalGrowthRecordId { get; set; }

    public int UserId { get; set; }

    public int? Week { get; set; }

    public DateTime? AlertDate { get; set; }

    public string Issue { get; set; }

    public string Severity { get; set; }

    public string Recommendation { get; set; }

    public bool? IsResolved { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual FetalGrowthRecord FetalGrowthRecord { get; set; }

    public virtual User User { get; set; }
}
