using PregnaCare.Core.Models;

namespace PregnaCare.Core.Repositories.Interfaces
{
    public interface INotificationRepository : IGenericRepository<Notification, Guid>
    {
        Task<List<Notification>> GetUserNotificationsAsync(Guid userId);
    }
}
