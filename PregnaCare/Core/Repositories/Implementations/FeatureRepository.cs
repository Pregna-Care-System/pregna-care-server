using Microsoft.EntityFrameworkCore;
using PregnaCare.Common.Enums;
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
            //check expired time
            var exiredMemberships = await _appDbContext.UserMembershipPlans.
                Where(x => x.ExpiryDate <= DateTime.Now && x.IsActive == true && x.IsDeleted == false).ToListAsync();
            //update active
            foreach (var membership in exiredMemberships)
            {
                membership.IsActive = false;

                // assign guest role 
                var userRole = await _appDbContext.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == membership.UserId);

                if (userRole != null)
                {
                    var guestRole = await _appDbContext.Roles
                .FirstOrDefaultAsync(r => r.RoleName == RoleEnum.Guest.ToString());

                    if (guestRole != null && userRole.RoleId != guestRole.Id)
                    {
                        userRole.RoleId = guestRole.Id;
                    }
                }
            }
            _ = await _appDbContext.SaveChangesAsync();
            // retrieve active membership plans for the user 
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
            if (!membershipPlansByUserId.Any())
            {
                return Enumerable.Empty<Feature>();
            }
            var featureMembershipPlanIds = membershipPlansByUserId.Select(mp => mp.MembershipPlanId).ToList();
            // retrieve feature with user membership plan
            var features = _appDbContext.Features
                .AsNoTracking()
                .Where(x => x.MembershipPlanFeatures.Any(y => featureMembershipPlanIds.Contains(y.MembershipPlanId)))
                .ToList();

            return features;
        }
    }
}
