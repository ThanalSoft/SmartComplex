using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ThanalSoft.SmartComplex.Api.Services
{
    public class SentEmailUtil
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
                Credentials = new NetworkCredential("harisuru@gmail.com", "H@r1@remu")
            };

            using (var message = new MailMessage("noreply@sc.com", pToEmail))
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