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
                Total = currentCount,
                PercentageChange = percentageChange
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
                Total = currentCount,
                PercentageChange = percentageChange
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
                Total = currentCount,
                PercentageChange = percentageChange
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
                Total = currentRevenue,
                PercentageChange = percentageChange
            };
        }
    }
}
