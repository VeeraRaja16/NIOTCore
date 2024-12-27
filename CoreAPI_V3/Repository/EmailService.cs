using System.Net.Mail;
using System.Net;
using CoreAPI_V3.Interface;

namespace CoreAPI_V3.Repository
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool SendEmailLog(string emailId, string subject, string messageBody)
        {
            try
            {
                var emailSettings = _configuration.GetSection("EmailSettings");
                string email = emailSettings["Email"];
                string password = emailSettings["Password"];
                string smtp = emailSettings["Smtp"];
                int port = int.Parse(emailSettings["Port"]);
                string appName = emailSettings["AppName"];

                NetworkCredential credentials = new NetworkCredential(email, password);
                MailMessage message = new MailMessage();
                MailAddress from = new MailAddress(email, appName);
                message.From = from;

                message.To.Add(new MailAddress(emailId));
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = messageBody;

                SmtpClient smtpClient = new SmtpClient(smtp)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Port = port,
                    Credentials = credentials
                };

                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                SendErrorEmail(ex.Message);
                throw;
            }
        }

        private void SendErrorEmail(string errorMessage)
        {
            try
            {
                var emailSettings = _configuration.GetSection("EmailSettings");
                string email = emailSettings["Email"];
                string password = emailSettings["Password"];
                string smtp = emailSettings["Smtp"];
                int port = int.Parse(emailSettings["Port"]);
                string emailrecipient = emailSettings["MailRecipient"];

                NetworkCredential credentials = new NetworkCredential(email, password);
                MailMessage message = new MailMessage();
                MailAddress from = new MailAddress(email);
                message.From = from;

                message.To.Add(new MailAddress(emailrecipient));
                message.Subject = "Error in Email Log";
                message.IsBodyHtml = true;
                message.Body = errorMessage;

                SmtpClient smtpClient = new SmtpClient(smtp)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Port = port,
                    Credentials = credentials
                };

                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending fallback email", ex);
            }
        }
    }
}
