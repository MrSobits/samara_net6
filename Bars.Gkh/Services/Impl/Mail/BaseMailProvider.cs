namespace Bars.Gkh.Services.Impl
{
    using System;
    using System.IO;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Administration;
    using Bars.Gkh.Enums.Administration.EmailMessage;
    using Bars.Gkh.Models;
    using Bars.Gkh.Services.ServiceContracts.Mail;

    using Newtonsoft.Json;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Базовый провайдер электроннй почты
    /// </summary>
    public abstract class BaseMailProvider<T> : IMailProvider<T> where T : class
    {
        #region Dependency Injection
        private readonly IPostalService postalService;
        private readonly IDomainService<EmailMessage> mailMessageDomain;
        private readonly IFileManager fileManager;

        public BaseMailProvider(IPostalService postalService,
            IDomainService<EmailMessage> mailMessageDomain,
            IFileManager fileManager)
        {
            this.postalService = postalService;
            this.mailMessageDomain = mailMessageDomain;
            this.fileManager = fileManager;
        }
        #endregion
        
        /// <inheritdoc />
        public abstract T PrepareData(BaseParams baseParams);

        /// <inheritdoc />
        public abstract string PrepareMessage(T mailData);

        /// <summary>
        /// Подготовить запись лога реестра писем
        /// </summary>
        protected abstract void PrepareLogRecord(EmailMessage message);

        /// <inheritdoc />
        public virtual void SendMessage(MailInfo mailInfo)
        {
            var logRecord = new EmailMessage
            {
                EmailAddress = mailInfo.RecieverMailAddress
            };

            this.PrepareLogRecord(logRecord);

            var jsonSerializer = new JsonSerializer();
            using(var ms = new MemoryStream())
            using(var sw = new StreamWriter(ms))
            using(var jsonWriter = new JsonTextWriter(sw))
            {
                FileInfo logFile = null;

                try
                {
                    logRecord.SendingTime = DateTime.Now;

                    this.postalService.Send(mailInfo);

                    logRecord.SendingStatus = EmailSendStatus.Success;
                    
                    jsonSerializer.Serialize(jsonWriter, new { Success = true });
                    jsonWriter.Flush();
                    ms.Seek(0, SeekOrigin.Begin);
                    
                    logFile = this.fileManager.SaveFile("успешно.json", ms.ReadAllBytes());
                }
                catch (Exception ex)
                {
                    logRecord.SendingStatus = EmailSendStatus.Error;

                    jsonSerializer.Serialize(jsonWriter, new { success = false, message = string.Join("; ", ex.Message, ex.InnerException?.Message)});
                    jsonWriter.Flush();
                    ms.Seek(0, SeekOrigin.Begin);
                    
                    logFile = this.fileManager.SaveFile("ошибка.json", ms.ReadAllBytes());

                    throw;
                }
                finally
                {
                    logRecord.LogFile = logFile;
                    
                    this.SaveLogResult(logRecord);
                }
            }
        }

        /// <summary>
        /// Сохранить результат логирования
        /// </summary>
        protected virtual void SaveLogResult(EmailMessage message)
        {
            this.mailMessageDomain.Save(message);
        }
    }
}