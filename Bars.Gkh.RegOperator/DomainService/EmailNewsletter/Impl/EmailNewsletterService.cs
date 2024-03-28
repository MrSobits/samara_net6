namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc
{
    using B4.Modules.FileStorage;
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Modules.Tasks.Common.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.RegOperator.DomainService.EmailNewsletter;
    using Bars.Gkh.RegOperator.Entities;
    using Castle.Windsor;
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;

    /// <summary>
    /// Сервис для работы со слепками документов на оплату
    /// </summary>
    public class EmailNewsletterService : IEmailNewsletterService
    {
        private readonly IWindsorContainer Container;

        /// <summary>
        /// Менеджер файлов
        /// </summary>
        public IFileManager FileManager { get; set; }

        public IDomainService<EmailNewsletter> EmailNewsletterDomain { get; set; }
        public IDomainService<EmailNewsletterLog> EmailNewsletterLogDomain { get; set; }

        public EmailNewsletterService(
           IWindsorContainer container, IFileManager fileManager,
           IDomainService<EmailNewsletter> emailNewsletterDomain,
           IDomainService<EmailNewsletterLog> emailNewsletterLogDomain)
        {
            Container = container;
            EmailNewsletterDomain = emailNewsletterDomain;
            EmailNewsletterLogDomain = emailNewsletterLogDomain;
            FileManager = fileManager;
        }

        /// <summary>
        /// Отправить документы на оплату по эл. почте
        /// </summary>
        /// <param name="params">Параметры запроса</param>
        /// <param name="indicator">Индикатор выполнения задачи</param>
        public IDataResult SendEmails(BaseParams @params, IProgressIndicator indicator)
        {
            var emailNewsletterId = @params.Params.GetAs<long>("emailNewsletterId");

            var emailNewsletter = EmailNewsletterDomain.Get(emailNewsletterId);

            var appSettings = ApplicationContext.Current.Configuration.AppSettings;
            var smtpClient = appSettings.GetAs<string>("smtpClient");
            var smtpPort = appSettings.GetAs<int>("smtpPort");
            var smtpEmail = appSettings.GetAs<string>("smtpEmail");
            var smtpLogin = appSettings.GetAs<string>("smtpLogin");
            var smtpPassword = appSettings.GetAs<string>("smtpPassword");

            indicator.Indicate(null, 0, "Инициализация данных");
            var destinationsEmail = emailNewsletter.Destinations.Split(',').ToList();

            indicator.Indicate(null, 70, "Отправка");
            foreach (var dest in destinationsEmail)
            {
                var emailNewsletterLog = new EmailNewsletterLog()
                {
                    EmailNewsletter = emailNewsletter,
                    Destination = dest
                };

                try
                {
                    using (var client = new SmtpClient(smtpClient, smtpPort))
                    using (var message = new MailMessage(smtpEmail, dest, emailNewsletter.Header, emailNewsletter.Body))
                    {
                        client.EnableSsl = true;
                        client.Credentials = new NetworkCredential(smtpLogin, smtpPassword);

                        if (emailNewsletter.Attachment != null)
                        {
                            var attachStream = FileManager.GetFile(emailNewsletter.Attachment);
                            attachStream.Position = 0;

                            using (attachStream)
                            using (var attachment = new Attachment(attachStream, emailNewsletter.Attachment.FullName, "application/pdf"))
                            {
                                message.Attachments.Add(attachment);


                                try
                                {
                                    client.Send(message);
                                }
                                catch
                                {
                                    try
                                    {
                                        client.EnableSsl = false;
                                        client.Send(message);
                                    }
                                    catch (Exception ex)
                                    {
                                        emailNewsletterLog.Log = $"При отправке произошла ошибка: {ex.Message}";
                                        emailNewsletterLog.Success = false;
                                        EmailNewsletterLogDomain.Save(emailNewsletterLog);
                                        continue;
                                    }
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                client.Send(message);
                            }
                            catch
                            {
                                try
                                {
                                    client.EnableSsl = false;
                                    client.Send(message);
                                }
                                catch (Exception ex)
                                {
                                    emailNewsletterLog.Log = $"При отправке произошла ошибка: {ex.Message}";
                                    emailNewsletterLog.Success = false;
                                    EmailNewsletterLogDomain.Save(emailNewsletterLog);
                                    continue;
                                }
                            }
                        }

                        emailNewsletterLog.Log = "Успешно отправлено";
                        emailNewsletterLog.Success = true;
                        EmailNewsletterLogDomain.Save(emailNewsletterLog);
                    }
                }
                catch (Exception e)
                {
                    emailNewsletterLog.Log = $"При отправке произошла ошибка: {e.Message}";
                    emailNewsletterLog.Success = false;
                    EmailNewsletterLogDomain.Save(emailNewsletterLog);
                }
            }
            emailNewsletter.Success = true;
            emailNewsletter.SendDate = DateTime.Now;
            EmailNewsletterDomain.Update(emailNewsletter);

            return new BaseDataResult();
        }
    }
}