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
        Task UpdateAllIsRead(List<Guid> ids);

        Task DeleteNotification(Guid id);
        Task<NotificationResponse> GetNotificationById(Guid id);
    }
}
