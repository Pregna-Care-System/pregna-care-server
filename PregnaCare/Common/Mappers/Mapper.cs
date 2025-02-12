using PregnaCare.Api.Models.Requests;
using PregnaCare.Core.DTOs;
using PregnaCare.Core.Models;

namespace PregnaCare.Common.Mappers
{
    public static class Mapper
    {
        public static MembershipPlan MapToMembershipPlan(MembershipPlanRequest request)
        {
            return new MembershipPlan
            {
                PlanName = request.PlanName,
                Price = request.Price,
                Duration = request.Duration,
                Description = request.Description,
                ImageUrl = request.ImageUrl
            };
        }
        public static Feature MapToFeature(FeatureRequest request)
        {
            return new Feature
            {
                FeatureName = request.FeatureName,
                Description = request.Description,
            };
        }
        public static MembershipPlanFeatureDTO MapToMembershipPlanDTO(MembershipPlan plan) 
        {
            return new MembershipPlanFeatureDTO
            {
                MembershipPlanId = plan.Id,
                PlanName = plan.PlanName,
                Price = plan.Price,
                Duration = plan.Duration,
                Description = plan.Description,
                CreatedAt = plan.CreatedAt,
                Features = plan.MembershipPlanFeatures.Select(x => new FeatureDTO
                {
                    FeatureName = x.Feature.FeatureName,
                    FeatureDescription = x.Feature.Description
                }).ToList()
            };
        }
    }
}
