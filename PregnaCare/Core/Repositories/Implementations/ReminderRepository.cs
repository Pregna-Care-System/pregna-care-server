﻿using Microsoft.EntityFrameworkCore;
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
            var currentDateTime = DateTime.UtcNow.Date;
            return await _appDbContext.Reminders.Where(f => (bool)!f.IsDeleted)
                .Where(f => f.ReminderDate >= currentDateTime)
                .OrderBy(f => f.ReminderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reminder>> GetRemindersToNotifyAsync(DateTime dateTime)
        {
            var notificationTime = dateTime.AddMinutes(30);
            return await _appDbContext.Reminders
                .Where(r => r.ReminderDate != null 
                && r.ReminderDate < notificationTime
                && r.Status == "Active" )
                .ToListAsync();
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
