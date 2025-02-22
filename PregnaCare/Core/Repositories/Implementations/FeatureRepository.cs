using Microsoft.EntityFrameworkCore;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;

namespace PregnaCare.Core.Repositories.Implementations
{
    public class FeatureRepository : GenericRepository<Feature, Guid>, IFeatureRepository
    {
        private readonly PregnaCareAppDbContext _appDbContext;

        public FeatureRepository(PregnaCareAppDbContext pregnaCareAppDbContext) : base(pregnaCareAppDbContext)
        {
            _appDbContext = pregnaCareAppDbContext;
        }

        public async Task<IEnumerable<Feature>> GetActiveFeatureAsync()
        {
            return await _appDbContext.Features.Where(f => (bool)!f.IsDeleted).ToListAsync();
        }

        public async Task<IEnumerable<Feature>> GetAllFeaturesByUserId(Guid userId)
        {
            var membershipPlansByUserId = _appDbContext.UserMembershipPlans
                                                      .AsNoTracking()
                                                      .Where(x => x.UserId == userId &&
                                                                  x.IsActive == true &&
                                                                  x.IsDeleted == false)
                                                      .Join(_appDbContext.MembershipPlans.AsNoTracking(),
                                                            usp => usp.MembershipPlanId,
                                                            mp => mp.Id,
                                                            (usp, mp) => new
                                                            {
                                                                usp.Id,
                                                                MembershipPlanId = mp.Id,
                                                                mp.PlanName,
                                                                mp.Price
                                                            })
                                                      .Where(x => x.Price == _appDbContext.MembershipPlans.AsNoTracking()
                                                            .Where(mp => mp.Id == x.MembershipPlanId)
                                                            .Max(mp => mp.Price))
                                                      .ToList();

            var featureMembershipPlanIds = membershipPlansByUserId.Select(mp => mp.MembershipPlanId).ToList();

            var features = _appDbContext.Features
                .AsNoTracking()
                .Where(x => x.MembershipPlanFeatures.Any(y => featureMembershipPlanIds.Contains(y.MembershipPlanId)))
                .ToList();

            return features;
        }
    }
}
