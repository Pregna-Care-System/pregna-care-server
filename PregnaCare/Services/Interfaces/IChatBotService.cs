namespace PregnaCare.Services.Interfaces
{
    public interface IChatBotService
    {
        Task<string> CallChatBotApi(string prompt);
    }
}
