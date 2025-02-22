using Microsoft.AspNetCore.SignalR;

namespace PregnaCare.Infrastructure.Hubs
{
    public class ReminderHub : Hub
    {
        public async Task SendReminder(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveReminder", message);
        }
    }
}
