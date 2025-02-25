using Microsoft.EntityFrameworkCore;
using PregnaCare.Core.DTOs;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;

namespace PregnaCare.Core.Repositories.Implementations
{
    public class AccountRepository : GenericRepository<User, Guid>, IAccountRepository
    {
        private readonly PregnaCareAppDbContext _appDbContext;
        public AccountRepository(PregnaCareAppDbContext pregnaCareAppDbContext) : base(pregnaCareAppDbContext)
        {
            _appDbContext = pregnaCareAppDbContext;
        }

        public async Task<IEnumerable<AccountDTO>?> GetMembers(string filterType = null, string name = null)
        {
            DateTime currentDate = DateTime.UtcNow;

            IQueryable<User> query = _appDbContext.Users
                .Where(u => u.IsDeleted == false && u.UserRoles.Any(ur => ur.Role.RoleName == "Member"));

            if (!string.IsNullOrEmpty(filterType))
            {
                if (filterType == "month")
                {
                    query = query.Where(u => u.CreatedAt.HasValue &&
                                             u.CreatedAt.Value.Month == currentDate.Month &&
                                             u.CreatedAt.Value.Year == currentDate.Year);
                }
                else if (filterType == "week")
                {
                    var startOfWeek = currentDate.AddDays(-(int)currentDate.DayOfWeek);
                    var endOfWeek = startOfWeek.AddDays(7);
                    query = query.Where(u => u.CreatedAt.HasValue &&
                                             u.CreatedAt.Value >= startOfWeek &&
                                             u.CreatedAt.Value < endOfWeek);
                }
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(u => u.FullName.Contains(name));
            }

            var members = await query
                .Include(u => u.UserMembershipPlans)
                    .ThenInclude(ump => ump.MembershipPlan)
                .Select(u => new AccountDTO
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Gender = u.Gender,
                    DateOfBirth = u.DateOfBirth,
                    Address = u.Address,
                    ImageUrl = u.ImageUrl,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt,
                    IsDeleted = u.IsDeleted,
                    IsActive = u.UserMembershipPlans
                        .Select(ump => ump.IsActive)
                         .FirstOrDefault(),
                    PlanName = u.UserMembershipPlans
                        .OrderByDescending(ump => ump.ExpiryDate)
                        .Select(ump => ump.MembershipPlan.PlanName)
                        .FirstOrDefault() ?? "No Plan",
                    remainingDate = u.UserMembershipPlans
                        .OrderByDescending(ump => ump.ExpiryDate)
                        .Select(ump => ump.ExpiryDate.HasValue
                            ? (ump.ExpiryDate.Value - DateTime.UtcNow).Days
                            : (int?)null)
                        .FirstOrDefault() ?? 0
                })
                .ToListAsync();

            // Nếu không có dữ liệu, trả về null thay vì danh sách rỗng
            return members.Any() ? members : null;
        }


    }
}
