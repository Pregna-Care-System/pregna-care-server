﻿using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests.FetalGrowthRecordRequestModel
{
    public class UpdateFetalGrowthRecordRequest : AbstractApiRequest
    {
        public Guid FetalGrowthRecordId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int? Week { get; set; }

        public double? Value { get; set; }

        public string Note { get; set; } = string.Empty;
    }
}
