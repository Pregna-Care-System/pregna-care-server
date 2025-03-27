using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Services.Interfaces;

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
                    var growthAlertService = scope.ServiceProvider.GetRequiredService<IGrowthAlertService>();

                    var now = DateTime.Now;
                    var reminders = await reminderRepository.GetRemindersToNotifyAsync(now);
                    var growthAlerts = await growthAlertService.GetFetalGrowthRecordsToSendNotification();

                    foreach (var growthAlert in growthAlerts)
                    {
                        await notificationService.SendReminderNotificationAsync(growthAlert.Id, growthAlert.UserId, "New Fetal Growth Update", "There's an important update regarding " + growthAlert.FetalGrowthRecord.Name + ". Please review the details to monitor your baby's health.");
                    }

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
