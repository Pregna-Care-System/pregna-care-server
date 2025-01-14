using PregnaCare.Services.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace PregnaCare.Services.Implementations
{
    public class EmailService:IEmailService
    {
        private string address = string.Empty;
        private string key = string.Empty;
        public EmailService(IConfiguration configuration) 
        {
            this.address = configuration.GetValue<string>("Email:Address")!;
            this.key = configuration.GetValue<string>("Email:AccessKey")!;
        }
        
        public bool SendEmail(string target, string subject, string body, string attachFile)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential(this.address, this.key);

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(this.address, "PregnaCare System");
                        mailMessage.To.Add(new MailAddress(target));
                        mailMessage.Subject = subject; 
                        mailMessage.Body = body;
                        mailMessage.IsBodyHtml = true;

                        if (!string.IsNullOrEmpty(attachFile))
                        {
                            mailMessage.Attachments.Add(new Attachment(attachFile));
                        }

                        smtpClient.Send(mailMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errors sending email:" +ex.ToString());
               return false;
            }
            return true;
        }



        //public async Task SendVerificationEmailAsync(string email, string userName, string verificationCode)
        //{
        //    string emailTemplate = File.ReadAllText("Utils/Html/SignupConfirmation.html");

        //    emailTemplate = emailTemplate
        //        .Replace("{UserName}", userName)
        //        .Replace("{VerificationCode}", verificationCode);

        //    // Send email
        //    await SendEmailAsyns(email, "SignupConfirmation", emailTemplate, null);
        //}



        
    }
}
