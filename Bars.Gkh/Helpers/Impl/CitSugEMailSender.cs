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
    internal class CitSugEMailSender : ICitSugEMailSender
    {
        public void Send(string to, string theme, string body, IEnumerable<Attachment> attachments = null)
        {
            var appSettings = ApplicationContext.Current.Configuration.AppSettings;
            var smtpClient = appSettings.GetAs<string>("CitSugSmtpClient");
            var smtpPort = appSettings.GetAs<int>("CitSugSmtpPort");
            var from = appSettings.GetAs<string>("CitSugSmtpEmail");
            var smtpLogin = appSettings.GetAs<string>("CitSugSmtpLogin");
            var smtpPassword = appSettings.GetAs<string>("CitSugSmtpPassword");
            var enableSsl = appSettings.GetAs<bool>("CitSugEnableSsl");

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
