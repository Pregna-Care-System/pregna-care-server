using PregnaCare.Api.Models.Requests.ReminderRequestModel;
using PregnaCare.Api.Models.Responses.ReminderResponseModel;

namespace PregnaCare.Services.Interfaces
{
    public interface IReminderService
    {
        Task CreateReminder(ReminderRequest request, Guid id);
        Task<ReminderListResponse> GetAllReminders();
        Task<ReminderListResponse> GetAllActiveReminders();
        Task UpdateReminder(Guid id, ReminderRequest request);
        Task DeleteReminder(Guid id);
        Task<ReminderResponse> GetReminderById(Guid id);
        Task<ReminderListResponse> GetAllRemindersByUserId(Guid userId);
    }
}
