using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class GrowthMetric
{
    public int GrowthMetricId { get; set; }

    public int PregnancyRecordId { get; set; }

    public DateOnly RecordedDate { get; set; }

    public decimal? FetalWeight { get; set; }

    public decimal? FetalHeight { get; set; }

    public int? FetalHeartRate { get; set; }

    public bool? IsFetalMovement { get; set; }

    public string FetalMovement { get; set; }

    public bool? IsFetalPosition { get; set; }

    public string Position { get; set; }

    public decimal? AmnioticFluidVolume { get; set; }

    public bool? IsFetalStructuralAbnormalities { get; set; }

    public string FetalStructuralAbnormalities { get; set; }

    public decimal? MaternalWeightGain { get; set; }

    public string MaternalBloodPressure { get; set; }

    public decimal? MaternalBloodGlucose { get; set; }

    public bool? IsGestationalDiabetes { get; set; }

    public string GestationalDiabetes { get; set; }

    public bool? IsPreEclampsia { get; set; }

    public string PreEclampsia { get; set; }

    public string PregnancyStatus { get; set; }

    public virtual ICollection<GrowthAlert> GrowthAlerts { get; set; } = new List<GrowthAlert>();

    public virtual PregnancyRecord PregnancyRecord { get; set; }
}
