﻿namespace PregnaCare.Core.DTOs
{
    public class FeatureDTO
    {
        public Guid Id { get; set; }
        public string FeatureName { get; set; } = string.Empty;

        public string FeatureDescription { get; set; } = string.Empty;
    }
}
