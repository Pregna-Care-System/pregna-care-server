﻿using Microsoft.EntityFrameworkCore;
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

        public async Task DeletePlanAsync(Guid planId)
        {
            var plan = await _context.MembershipPlans
                .Include(mp => mp.MembershipPlanFeatures)
                .FirstOrDefaultAsync(mp => mp.Id == planId && mp.IsDeleted == false);
            if(plan != null)
            {
                plan.IsDeleted = true;
                plan.UpdatedAt = DateTime.UtcNow;
                
                foreach(var planFeature in plan.MembershipPlanFeatures)
                {
                    planFeature.IsDeleted = true;
                    planFeature.UpdatedAt = DateTime.UtcNow;
                }
            }
                
        }

        public async Task<MembershipPlanFeatureDTO> GetPlanById(Guid id)
        {
            var plan = await _context.MembershipPlans
                .Where(mp => mp.Id == id && mp.IsDeleted == false)
                .Include(mp => mp.MembershipPlanFeatures)
                    .ThenInclude(mpf => mpf.Feature)
                .FirstOrDefaultAsync();

            return new MembershipPlanFeatureDTO
            {
                MembershipPlanId = plan.Id,
                PlanName = plan.PlanName,
                Price = plan.Price,
                Duration = plan.Duration,
                Description = plan.Description,
                CreatedAt = plan.CreatedAt,
                Features = plan.MembershipPlanFeatures
                            .Where(mpf => mpf.IsDeleted == false)
                            .Select(mpf => new FeatureDTO
                            {
                                Id = mpf.Id,
                                FeatureName = mpf.Feature.FeatureName
                            }).ToList()
            };
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
                MembershipPlanId = plan.Id,
                PlanName = plan.PlanName,
                Price = plan.Price,
                Duration = plan.Duration,
                Description = plan.Description,
                CreatedAt = plan.CreatedAt,
                Features = plan.MembershipPlanFeatures
                            .Where(mpf => mpf.IsDeleted == false)
                            .Select(mpf => new FeatureDTO
                            {
                                Id = mpf.Id,
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
                MembershipPlanId = mp.Id,
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

        async Task IMembershipPlansRepository.Update(MembershipPlan plan, List<Guid> featureIds)
        {
            var existingPlan = await _context.MembershipPlans
                .Include(mp => mp.MembershipPlanFeatures)
                .FirstOrDefaultAsync(mp => mp.Id == plan.Id && mp.IsDeleted == false);

            if (existingPlan == null)
            {
                throw new Exception("Plan not found");
            }

            existingPlan.PlanName = plan.PlanName;
            existingPlan.Price = plan.Price;
            existingPlan.Duration = plan.Duration;
            existingPlan.Description = plan.Description;
            existingPlan.UpdatedAt = DateTime.UtcNow;

            // Tìm các FeatureIds đã tồn tại trong MembershipPlan
            var existingFeatureIds = existingPlan.MembershipPlanFeatures
                .Where(mpf => mpf.IsDeleted == false)
                .Select(mpf => mpf.FeatureId)
                .ToList();

            existingPlan.MembershipPlanFeatures
                .Where(mpf => !featureIds.Contains(mpf.FeatureId) && mpf.IsDeleted == false)
                .ToList()
                .ForEach(mpf =>
                {
                    mpf.IsDeleted = true;
                    mpf.UpdatedAt = DateTime.UtcNow;
                });

            var newFeatureIds = featureIds.Except(existingFeatureIds);
            var newFeatures = newFeatureIds.Select(featureId => new MembershipPlanFeature
            {
                Id = Guid.NewGuid(),
                MembershipPlanId = plan.Id,
                FeatureId = featureId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            await _context.MembershipPlanFeatures.AddRangeAsync(newFeatures);

            await _context.SaveChangesAsync();
        }

    }
}
