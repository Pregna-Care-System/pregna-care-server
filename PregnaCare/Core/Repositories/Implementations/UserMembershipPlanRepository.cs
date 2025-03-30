using Microsoft.EntityFrameworkCore;
using PregnaCare.Core.DTOs;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;

namespace PregnaCare.Core.Repositories.Implementations
{
    public class UserMembershipPlanRepository : GenericRepository<UserMembershipPlan, Guid>, IUserMembershipPlanRepository
    {
        private readonly PregnaCareAppDbContext _context;
        public UserMembershipPlanRepository(PregnaCareAppDbContext pregnaCareAppDbContext) : base(pregnaCareAppDbContext)
        {
            _context = pregnaCareAppDbContext;
        }

        public async Task<IEnumerable<UserMembershipPlanDTO>> GetUserMembershipPlanList()
        {
            return await _context.UserMembershipPlans
               .Include(ump => ump.MembershipPlan)
               .Include(ump => ump.User)
               .Select(ump => new UserMembershipPlanDTO
               {
                   Id = ump.Id,
                   UserId = ump.UserId,
                   MembershipPlanId = ump.MembershipPlanId,
                   Email = ump.User.Email,
                   FullName = ump.User.FullName,
                   MembershipPlanName = ump.MembershipPlan.PlanName,
                   ActivatedAt = ump.ActivatedAt,
                   ExpiryDate = ump.ExpiryDate,
                   Price = ump.Price,
                   IsActive = ump.IsActive,
                   IsDeleted = ump.IsDeleted
               })
               .ToListAsync();

        }

        public async Task<IEnumerable<UserMembershipPlanDTO>> GetUserTransactions(Guid userId)
        {
            return await _context.UserMembershipPlans
                .Include(ump => ump.MembershipPlan)
                 .Where(ump => ump.UserId == userId)
                .Select(ump => new UserMembershipPlanDTO
                {
                    Id = ump.Id,
                    UserId = ump.UserId,
                    MembershipPlanId = ump.MembershipPlanId,
                    MembershipPlanName = ump.MembershipPlan.PlanName,
                    ActivatedAt = ump.ActivatedAt,
                    ExpiryDate = ump.ExpiryDate,
                    Price = ump.Price,
                    IsActive = ump.IsActive,
                    IsDeleted = ump.IsDeleted
                })
                .OrderByDescending(ump => ump.ActivatedAt)
                .ToListAsync();
        }
    }
}
