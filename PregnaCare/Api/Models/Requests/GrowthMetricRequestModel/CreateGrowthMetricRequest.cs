﻿using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.GrowthMetricRequestModel
{
    public class CreateGrowthMetricRequest : AbstractApiRequest
    {
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
        public double? MinValue { get; set; }
        public double? MaxValue { get; set; }
        public int? Week { get; set; }
    }
}
