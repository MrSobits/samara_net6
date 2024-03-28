using Bars.B4.Application;
using Bars.B4.Utils;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace Bars.Gkh.Helpers.Impl
{
    /// <summary>
    /// Отправка писем, не привязанная ни к какому модулю
    /// </summary>
    internal class EMailSender : IEMailSender
    {
        public void Send(string to, string theme, string body, IEnumerable<Attachment> attachments = null)
        {
            var appSettings = ApplicationContext.Current.Configuration.AppSettings;
            var smtpClient = appSettings.GetAs<string>("smtpClient");
            var smtpPort = appSettings.GetAs<int>("smtpPort");
            var from = appSettings.GetAs<string>("smtpEmail");
            var smtpLogin = appSettings.GetAs<string>("smtpLogin");
            var smtpPassword = appSettings.GetAs<string>("smtpPassword");
            var enableSsl = appSettings.GetAs<bool>("enableSsl");

            using (var client = new SmtpClient(smtpClient, smtpPort))
            {               
                client.EnableSsl = enableSsl;
                client.Credentials = new NetworkCredential(smtpLogin, smtpPassword);
                var message = new MailMessage(from, to, theme, body);

                if (attachments != null)
                    attachments.ForEach(x => message.Attachments.Add(x));
                
                client.Send(message);
            }
        }

    }
}
