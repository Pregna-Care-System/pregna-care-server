using Microsoft.EntityFrameworkCore;
using PregnaCare.Common.Enums;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;

namespace PregnaCare.Core.Repositories.Implementations
{
    public class ReminderRepository : GenericRepository<Reminder, Guid>, IReminderRepository
    {
        private readonly PregnaCareAppDbContext _appDbContext;
        public ReminderRepository(PregnaCareAppDbContext pregnaCareAppDbContext) : base(pregnaCareAppDbContext)
        {
            _appDbContext = pregnaCareAppDbContext;
        }

        public async Task<IEnumerable<Reminder>> GetActiveRemindersAsync()
        {
            var currentDateTime = DateTime.Now.Date;
            return await _appDbContext.Reminders.Where(f => (bool)!f.IsDeleted)
                .Where(f => f.ReminderDate >= currentDateTime)
                .OrderBy(f => f.ReminderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reminder>> GetRemindersToNotifyAsync(DateTime dateTime)
        {
            var notifyTimes = new List<TimeSpan>
            {
                //TimeSpan.FromMinutes(60), 
                TimeSpan.FromMinutes(30),
                TimeSpan.FromMinutes(5)
            };

            return (await _appDbContext.Reminders
                        .ToListAsync())
                        .Where(r =>     r.ReminderDate != null
                                    && r.ReminderDate.Value.Date == dateTime.Date
                                    && notifyTimes.Any(nt => Math.Abs((r.ReminderDate.Value - nt - dateTime).TotalMinutes) < 1)
                                    && r.Status == StatusEnum.Active.ToString()).ToList();
        }


        public async Task<IEnumerable<Guid>> GetUserIdsForReminderAsync(Guid reminderId)
        {
            return await _appDbContext.UserReminders
                .Where(ur => ur.ReminderId == reminderId)
                .Select(ur => ur.UserId)
                .ToListAsync();
        }
    }
}
