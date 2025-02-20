using Microsoft.EntityFrameworkCore;
using PregnaCare.Api.Models.Responses.StatisticsResponseModel;
using PregnaCare.Common.Enums;
using PregnaCare.Infrastructure.Data;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class StatisticsService : IStatisticsService
    {
        private readonly PregnaCareAppDbContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public StatisticsService(PregnaCareAppDbContext context)
        {
            _context = context;
        }

        public async Task<StatsResponse> GetMemberStatisticsAsync()
        {
            var currentCount = await _context.Users.Include(x => x.UserRoles).CountAsync(x => x.IsDeleted == false && x.UserRoles.Any(y => y.Role.RoleName != RoleEnum.Admin.ToString()));
            var thirtyDaysAgo = DateTime.Now.AddDays(-30);
            var pastCount = await _context.Users.CountAsync(x => x.CreatedAt <= thirtyDaysAgo && x.IsDeleted == false && x.UserRoles.Any(y => y.Role.RoleName != RoleEnum.Admin.ToString()));
            var percentageChange = pastCount == 0 ? 0 : ((double)(currentCount - pastCount) / pastCount) * 100;

            return new StatsResponse
            {
                Title = "Total Members",
                Total = currentCount,
                PercentageChange = percentageChange,
                IsIncrease = percentageChange > 0
            };
        }

        public async Task<StatsResponse> GetUserStatisticsAsync()
        {
            var currentCount = await _context.Users.Include(x => x.UserRoles).CountAsync(x => x.IsDeleted == false);
            var thirtyDaysAgo = DateTime.Now.AddDays(-30);
            var pastCount = await _context.Users.CountAsync(x => x.CreatedAt <= thirtyDaysAgo && x.IsDeleted == false);
            var percentageChange = pastCount == 0 ? 0 : ((double)(currentCount - pastCount) / pastCount) * 100;

            return new StatsResponse
            {
                Title = "Total Users",
                Total = currentCount,
                PercentageChange = percentageChange,
                IsIncrease = percentageChange > 0
            };
        }

        public async Task<StatsResponse> GetTotalTransactionStatisticsAsync()
        {
            var currentCount = await _context.UserMembershipPlans.CountAsync(x => x.IsDeleted == false);
            var thirtyDaysAgo = DateTime.Now.AddDays(-30);
            var pastCount = await _context.UserMembershipPlans.CountAsync(x => x.CreatedAt <= thirtyDaysAgo && x.IsDeleted == false);
            var percentageChange = pastCount == 0 ? 0 : ((double)(currentCount - pastCount) / pastCount) * 100;

            return new StatsResponse
            {
                Title = "Total Transactions",
                Total = currentCount,
                PercentageChange = percentageChange,
                IsIncrease = percentageChange > 0
            };
        }

        public async Task<StatsResponse> GetTotalRevenueStatisticsAsync()
        {
            var currentRevenue = await _context.UserMembershipPlans
                                             .AsNoTracking()
                                             .Where(x => x.IsDeleted == false)
                                             .SumAsync(x => x.Price);

            var thirtyDaysAgo = DateTime.Now.AddDays(-30);
            var pastRevenue = await _context.UserMembershipPlans
                                            .AsNoTracking()
                                            .Where(x => x.IsDeleted == false && x.CreatedAt <= thirtyDaysAgo)
                                            .SumAsync(x => x.Price);
            var percentageChange = pastRevenue == 0
                ? 0
                : ((double)(currentRevenue - pastRevenue) / (double)pastRevenue) * 100;

            return new StatsResponse
            {
                Title = "Total Revenue",
                Total = currentRevenue,
                PercentageChange = percentageChange,
                IsIncrease = percentageChange > 0
            };
        }

        public async Task<List<MembershipStatsResponse>> GetMembershipStatsAsync()
        {
            var stats = await (from plan in _context.MembershipPlans
                               join userPlan in _context.UserMembershipPlans on plan.Id equals userPlan.MembershipPlanId
                               where !plan.IsDeleted.Value &&
                                     !userPlan.IsDeleted.Value &&
                                     userPlan.IsActive.Value
                               group userPlan by plan.PlanName into g
                               select new
                               {
                                   Name = g.Key,
                                   Users = g.Count()
                               }).ToListAsync();

            var totalUsers = stats.Sum(s => s.Users);

            var membershipPlans = stats.Select(s => new MembershipStatsResponse
            {
                Name = s.Name,
                Users = s.Users,
                Percentage = totalUsers > 0 ? ((double)s.Users / totalUsers * 100).ToString("F1") + "%" : "0%"
            }).ToList();

            return membershipPlans;
        }

        public async Task<(int, int, int, List<TransactionStatsResponse>)> GetRecentTransactionsAsync(int offset, int limit)
        {
            if (offset < 0 || limit < 1) return (0, 0, 0, null);

            var totalTransactions = await _context.UserMembershipPlans.AsNoTracking()
                                                  .Where(up => up.IsDeleted == false)
                                                  .CountAsync();

            var transactions = await (from userPlan in _context.UserMembershipPlans
                                      join plan in _context.MembershipPlans on userPlan.MembershipPlanId equals plan.Id
                                      join user in _context.Users on userPlan.UserId equals user.Id
                                      where !userPlan.IsDeleted.Value && userPlan.IsActive.Value
                                      orderby userPlan.CreatedAt descending
                                      select new TransactionStatsResponse
                                      {
                                          FullName = user.FullName,
                                          MembershipPlan = plan.PlanName,
                                          Price = userPlan.Price.ToString(),
                                          Status = userPlan.Status,
                                          BuyDate = (DateTime)userPlan.CreatedAt
                                      }).Skip(offset).Take(limit).ToListAsync();

            return (totalTransactions, offset, limit, transactions);
        }
    }
}
