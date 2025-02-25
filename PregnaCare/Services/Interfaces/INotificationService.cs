using PregnaCare.Api.Models.Responses.NotificationResponseModel;

namespace PregnaCare.Services.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationListResponse> GetAllNotificationsByUserId(Guid userId);
        Task UpdateIsReadNotification(Guid id);
        Task DeleteNotification(Guid id);
        Task<NotificationResponse> GetNotificationById(Guid id);
    }
}
