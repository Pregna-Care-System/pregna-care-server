using PregnaCare.Api.Models.Responses.NotificationResponseModel;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.UnitOfWork;
using PregnaCare.Services.Interfaces;

namespace PregnaCare.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notiRepo;
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(INotificationRepository notiRepo, IUnitOfWork unitOfWork)
        {
            _notiRepo = notiRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteNotification(Guid id)
        {
            var noti = await _notiRepo.GetByIdAsync(id);
            if (noti != null)
            {
                noti.IsDeleted = true;
                _notiRepo.Update(noti);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<NotificationListResponse> GetAllNotificationsByUserId(Guid userId)
        {
            var userNotification = await _notiRepo.GetUserNotificationsAsync(userId);
            return new NotificationListResponse
            {
                Success = true,
                Response = userNotification
            };
        }

        public async Task<NotificationResponse> GetNotificationById(Guid id)
        {
            var noti = await _notiRepo.GetByIdAsync(id);
            if (noti != null)
            {
                return new NotificationResponse { Response = noti, Success = true };
            }
            return null;
        }

        public async Task UpdateAllIsRead(List<Guid> ids)
        {
            var notifications = await _notiRepo.FindAsync(n => ids.Contains(n.Id));
            foreach (var noti in notifications)
            {
                noti.IsRead = true;
                _notiRepo.Update(noti);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateIsReadNotification(Guid id)
        {
            var noti = await _notiRepo.GetByIdAsync(id);
            if (noti != null)
            {
                noti.IsRead = true;
                _notiRepo.Update(noti);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
