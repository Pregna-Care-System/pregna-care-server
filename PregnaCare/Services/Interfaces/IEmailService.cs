namespace PregnaCare.Services.Interfaces
{
    public interface IEmailService
    {
        bool SendEmail(string target, string subject, string body, string attachFile);
    }
}
