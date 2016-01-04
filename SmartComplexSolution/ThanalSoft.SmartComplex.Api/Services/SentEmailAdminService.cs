using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ThanalSoft.SmartComplex.Api.Services
{
    public class SentEmailAdminService
    {
        public async Task SendEmailAsync(string pToEmail, string pSubject, string pBody, bool pIsHtmlBody = true)
        {
            var smtp = new SmtpClient
            {
                Host = GmailHost,
                Port = GmailPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("noreplysmartcomplex@gmail.com", "smartcomplex123")
            };

            using (var message = new MailMessage("noreplysmartcomplex@gmail.com", pToEmail))
            {
                message.Subject = pSubject;
                message.Body = pBody;
                message.IsBodyHtml = pIsHtmlBody;
                await smtp.SendMailAsync(message);
            }
        }

        private int GmailPort => 25;

        private string GmailHost => "smtp.gmail.com";

    }
}