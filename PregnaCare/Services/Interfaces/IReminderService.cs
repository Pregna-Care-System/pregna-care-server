using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;

namespace PregnaCare.Services.Interfaces
{
    public interface IReminderService
    {
        Task CreateReminder(ReminderRequest request);
        Task<ReminderListResponse> GetAllReminders();
        Task<ReminderListResponse> GetAllActiveReminders();
        Task UpdateReminder(Guid id, ReminderRequest request);
        Task DeleteReminder(Guid id);
        Task<ReminderResponse> GetReminderById(Guid id);

    }
}
