using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;
using PregnaCare.Infrastructure.Hubs;
using PregnaCare.Services.Interfaces;
using System;

namespace PregnaCare.Services.BackgroundServices
{
    public class ReminderBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ReminderBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var reminderRepository = scope.ServiceProvider.GetRequiredService<IReminderRepository>();
                    var notificationService = scope.ServiceProvider.GetRequiredService<IReminderNotificationService>();
                    var now = DateTime.Now;
                    var reminders = await reminderRepository.GetRemindersToNotifyAsync(now);

                    foreach (var reminder in reminders)
                    {
                        var userIds = await reminderRepository.GetUserIdsForReminderAsync(reminder.Id);
                        foreach (var userId in userIds)
                        {
                            await notificationService.SendReminderNotificationAsync(userId, reminder.Title, $"{reminder.Title} {reminder.ReminderDate}");
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
