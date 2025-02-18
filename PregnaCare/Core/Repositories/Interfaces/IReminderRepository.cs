using PregnaCare.Core.Models;

namespace PregnaCare.Core.Repositories.Interfaces
{
    public interface IReminderRepository : IGenericRepository<Reminder, Guid>
    {
        Task<IEnumerable<Reminder>> GetActiveRemindersAsync();
    }
}
