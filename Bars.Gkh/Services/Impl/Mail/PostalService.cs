namespace Bars.Gkh.Services.Impl
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using System.Threading;
    using System.Threading.Tasks;

    using Bars.B4.Config;
    using Bars.Gkh.ConfigSections.PostalService;
    using Bars.Gkh.Models;
    using Bars.Gkh.Services.ServiceContracts.Mail;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Почтовый сервис
    /// </summary>
    public class PostalService : IPostalService
    {
        /// <summary>
        /// ЖКХ конфигурация почтового сервиса
        /// </summary>
        private PostalServiceConfig postalServiceGkhConfig;

        /// <summary>
        /// Включен ли почтовый сервис в конфигурации приложения
        /// </summary>
        /// <returns></returns>
        private bool isPostalServiceEnableInCfg;

        public PostalService(IWindsorContainer container)
        {
            postalServiceGkhConfig = container.GetGkhConfig<PostalServiceConfig>();
            isPostalServiceEnableInCfg = container.Resolve<IConfigProvider>().GetConfig().AppSettings.GetAs<bool>("EmailServiceEnabled");
        }

        /// <inheritdoc />
        public void Send(MailInfo mailInfo)
        {
            if (!postalServiceGkhConfig.EnablePostalService || !isPostalServiceEnableInCfg)
            {
                throw new Exception("Почтовый сервис отключен");
            }

            var smtp = this.PrepareSmtpClient();
            var message = PrepareMessage(mailInfo.RecieverMailAddress, mailInfo.MailTheme, mailInfo.MessageBody);

            smtp.Send(message);
        }

        /// <inheritdoc />
        public async Task SendAsync(MailInfo mailInfo)
        {
            if (!postalServiceGkhConfig.EnablePostalService || !isPostalServiceEnableInCfg)
            {
                throw new Exception("Почтовый сервис отключен");
            }
            
            var smtp = this.PrepareSmtpClient();
            var message = PrepareMessage(mailInfo.RecieverMailAddress, mailInfo.MailTheme, mailInfo.MessageBody);

            CancellationTokenSource cts = new CancellationTokenSource();
            await Task.Run(() => smtp.SendAsync(message, cts.Token), cts.Token);
        }

        private SmtpClient PrepareSmtpClient()
        {
            return new SmtpClient(postalServiceGkhConfig.SMTPServerAddress, Int32.Parse(postalServiceGkhConfig.SenderPostPort))
            {
                Credentials = new NetworkCredential(postalServiceGkhConfig.Login, postalServiceGkhConfig.Password)
            };
        }

        private MailMessage PrepareMessage(string recieverPost, string header, string messageBody)
        {
            return new MailMessage(new MailAddress(postalServiceGkhConfig.SenderPost, "ГИС МЖФ"), new MailAddress(recieverPost))
            {
                Subject = header,
                Body = messageBody,
                IsBodyHtml = true
            };
        }
    }
}