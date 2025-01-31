using Microsoft.EntityFrameworkCore;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;

namespace PregnaCare.Core.Repositories.Implementations
{
    public class MembershipPlansRepository : GenericRepository<MembershipPlan, Guid>, IMembershipPlansRepository
    {
        private readonly PregnaCareAppDbContext _context;
        public MembershipPlansRepository(PregnaCareAppDbContext pregnaCareAppDbContext) : base(pregnaCareAppDbContext)
        {
            _context = pregnaCareAppDbContext;
        }

        public async Task<IEnumerable<MembershipPlan>> GetActivePlanAsync()
        {
            return await _context.MembershipPlans.Where(mp => (bool)!mp.IsDeleted).ToListAsync();
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
    }
}
