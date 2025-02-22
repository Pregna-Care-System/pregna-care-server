using PregnaCare.Core.Models;

namespace PregnaCare.Core.Repositories.Interfaces
{
    public interface IReminderRepository : IGenericRepository<Reminder, Guid>
    {
        Task<IEnumerable<Reminder>> GetActiveRemindersAsync();
        Task<IEnumerable<Reminder>> GetRemindersToNotifyAsync(DateTime dateTime);
        Task<IEnumerable<Guid>> GetUserIdsForReminderAsync(Guid reminderId);


    }
}
