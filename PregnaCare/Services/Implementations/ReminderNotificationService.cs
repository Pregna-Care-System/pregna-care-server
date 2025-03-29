using Microsoft.AspNetCore.SignalR;
using PregnaCare.Common.Enums;
using PregnaCare.Core.Models;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Hubs;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class ReminderNotificationService : IReminderNotificationService
    {
        private readonly IHubContext<ReminderHub> _hubContext;
        private readonly IGenericRepository<Notification, Guid> _notificationRepo;
        private readonly IUnitOfWork _unitOfWork;

        public ReminderNotificationService(IHubContext<ReminderHub> hubContext, IUnitOfWork unitOfWork)
        {
            _hubContext = hubContext;
            _unitOfWork = unitOfWork;
            _notificationRepo = _unitOfWork.GetRepository<Notification, Guid>();
        }

        public async Task SendReminderNotificationAsync(Guid userId, string title, string message)
        {
            await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveReminder", message);

            var notificationEntity = (await _notificationRepo.FindAsync(x => x.ReceiverId == userId &&
                                                                            x.Title == title &&
                                                                            x.Message == message &&
                                                                            x.IsRead == false &&
                                                                            x.IsDeleted == false &&
                                                                            x.Status == StatusEnum.Pending.ToString()))
                                                             .FirstOrDefault();
            if (notificationEntity != null)
            {
                return;
            }

            var notification = new Notification
            {
                ReceiverId = userId,
                Title = title,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = StatusEnum.Pending.ToString(),
            };

            await _notificationRepo.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SendReminderNotificationAsync(Guid senderId, Guid receiverId, string title, string message)
        {
            await _hubContext.Clients.User(receiverId.ToString()).SendAsync("ReceiveReminder", message);

            var notificationEntity = (await _notificationRepo.FindAsync(x => x.ReceiverId == receiverId &&
                                                                            x.Title == title &&
                                                                            x.Message == message &&
                                                                            x.IsRead == false &&
                                                                            x.IsDeleted == false &&
                                                                            x.Status == StatusEnum.Pending.ToString()))
                                                             .FirstOrDefault();
            if (notificationEntity != null)
            {
                return;
            }

            var notification = new Notification
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Title = title,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = StatusEnum.Pending.ToString(),
            };

            await _notificationRepo.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
