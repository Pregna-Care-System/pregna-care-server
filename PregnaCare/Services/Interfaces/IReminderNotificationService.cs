namespace PregnaCare.Services.Interfaces
{
    public interface IReminderNotificationService
    {
        Task SendReminderNotificationAsync(Guid userId, string title, string message);
        Task SendReminderNotificationAsync(Guid senderId, Guid receiverId, string title, string message);
    }
}
