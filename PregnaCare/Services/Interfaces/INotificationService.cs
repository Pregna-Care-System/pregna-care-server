using PregnaCare.Api.Models.Requests.ReminderRequestModel;
using PregnaCare.Api.Models.Responses.NotificationResponseModel;
using PregnaCare.Api.Models.Responses.ReminderResponseModel;
using PregnaCare.Core.Models;

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
