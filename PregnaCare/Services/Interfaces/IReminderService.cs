using PregnaCare.Api.Models.Requests;
using PregnaCare.Api.Models.Responses;

namespace PregnaCare.Services.Interfaces
{
    public interface IReminderService
    {
        Task CreateReminder(ReminderRequest request);
        Task<IEnumerable<ReminderResponse>> GetAllReminder();
        Task UpdateReminder(Guid id, ReminderRequest request);
        Task DeleteReminder(Guid id);
        Task<ReminderResponse> GetReminderById(Guid id);

    }
}
