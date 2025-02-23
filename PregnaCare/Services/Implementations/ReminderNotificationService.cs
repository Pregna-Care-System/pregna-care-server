using Microsoft.AspNetCore.SignalR;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Implementations;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Hubs;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class ReminderNotificationService : IReminderNotificationService
    {
        private readonly IHubContext<ReminderHub> _hubContext;
        private readonly INotificationRepository _notificationRepo;

        public ReminderNotificationService(IHubContext<ReminderHub> hubContext, INotificationRepository notificationRepository)
        {
            _hubContext = hubContext;
            _notificationRepo = notificationRepository;
        }
        public async Task SendReminderNotificationAsync(Guid userId, string title, string message)
        {
            await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveReminder", message);

            var notification = new Notification
            {
                ReceiverId = userId,
                Title = title,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = "Pending"
            };
            await _notificationRepo.AddAsync(notification);
        }
    }
}
