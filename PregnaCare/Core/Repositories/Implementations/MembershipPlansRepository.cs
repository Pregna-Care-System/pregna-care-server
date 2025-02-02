using Microsoft.EntityFrameworkCore;
using PregnaCare.Core.DTOs;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;
using System.Numerics;

namespace PregnaCare.Core.Repositories.Implementations
{
    public class MembershipPlansRepository : GenericRepository<MembershipPlan, Guid>, IMembershipPlansRepository
    {
        private readonly PregnaCareAppDbContext _context;
        public MembershipPlansRepository(PregnaCareAppDbContext pregnaCareAppDbContext) : base(pregnaCareAppDbContext)
        {
            _context = pregnaCareAppDbContext;
        }

        public async Task AddPlanAsync(MembershipPlan plan, List<Guid> featureIds)
        {
            _context.MembershipPlans.Add(plan);

            foreach (var featureId in featureIds)
            {
                var planFeature = new MembershipPlanFeature
                {
                    Id = Guid.NewGuid(),
                    MembershipPlanId = plan.Id,
                    FeatureId = featureId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.MembershipPlanFeatures.Add(planFeature);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<MembershipPlanFeatureDTO> GetPlanByName(string name)
        {
            var plan = await _context.MembershipPlans
                .Where(mp => mp.PlanName == name && mp.IsDeleted == false)
                .Include(mp => mp.MembershipPlanFeatures)
                    .ThenInclude(mpf => mpf.Feature)
                .FirstOrDefaultAsync();

            return new MembershipPlanFeatureDTO
            {
                PlanName = plan.PlanName,
                Price = plan.Price,
                Duration = plan.Duration,
                Description = plan.Description,
                CreatedAt = plan.CreatedAt,
                Features = plan.MembershipPlanFeatures
                            .Where(mpf => mpf.IsDeleted == false)
                            .Select(mpf => new FeatureDTO
                            {
                                FeatureName = mpf.Feature.FeatureName
                            }).ToList()
            };
        }

        public async Task<IEnumerable<MembershipPlanFeatureDTO>> GetPlansWithFeaturesAsync()
        {
            var plans = await _context.MembershipPlans
                                        .Where(mp => mp.IsDeleted == false)
                                        .Include(mp => mp.MembershipPlanFeatures)
                                            .ThenInclude(mpf => mpf.Feature)
                                        .ToListAsync();

            var plansWithFeatures = plans.Select(mp => new MembershipPlanFeatureDTO
            {
                PlanName = mp.PlanName,
                Price = mp.Price,
                Duration = mp.Duration,
                Description = mp.Description,
                CreatedAt = mp.CreatedAt,
                Features = mp.MembershipPlanFeatures
                            .Where(mpf => mpf.IsDeleted == false)
                            .Select(mpf => new FeatureDTO
                            {
                                FeatureName = mpf.Feature.FeatureName
                            }).ToList()
            }).ToList();

            return plansWithFeatures;
        }



    }
}
