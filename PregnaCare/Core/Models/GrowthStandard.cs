using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class GrowthStandard
{
    public int GrowthStandardId { get; set; }

    public string Indicator { get; set; }

    public decimal? MinStandard { get; set; }

    public decimal? MaxStandard { get; set; }

    public string Unit { get; set; }

    public string WarningMessage { get; set; }

    public string Category { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string ModifiedBy { get; set; }
}
