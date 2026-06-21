using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OnlineCompilerWebForms.Services
{
    public class EmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var host = ConfigurationManager.AppSettings["SmtpHost"];
            var port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            var user = ConfigurationManager.AppSettings["SmtpUser"];
            var pass = ConfigurationManager.AppSettings["SmtpPassword"];
            var enableSsl = bool.Parse(ConfigurationManager.AppSettings["SmtpEnableSsl"]);

            using (var client = new SmtpClient(host, port))
            {
                client.Credentials = new NetworkCredential(user, pass);
                client.EnableSsl = enableSsl;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(user, "MouSou Support"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
