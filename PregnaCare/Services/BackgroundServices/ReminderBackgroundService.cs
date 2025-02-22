using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PregnaCare.Infrastructure.Data;
using PregnaCare.Infrastructure.Hubs;
using System;

namespace PregnaCare.Services.BackgroundServices
{
    public class ReminderBackgroundService: BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<ReminderHub> _hubContext;

        public ReminderBackgroundService(IServiceScopeFactory scopeFactory, IHubContext<ReminderHub> hubContext)
        {
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<PregnaCareAppDbContext>();
                    var now = DateTime.Now;

                    var upcomingReminders = await dbContext.Reminders
                        .Where(r => r.ReminderDate <= now.AddMinutes(30) && r.ReminderDate > now && r.Status == "Active")
                        .ToListAsync();

                    //foreach (var reminder in upcomingReminders)
                    //{
                    //    await _hubContext.Clients.User(reminder.UserId.ToString())
                    //        .SendAsync("ReceiveReminder", $"Reminder: {reminder.Title} is due at {reminder.ReminderDate:hh:mm tt}");
                    //}
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
        }
}
