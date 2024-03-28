namespace Bars.GkhGji.Regions.Chelyabinsk.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Entities;
    using Bars.GkhGji.Entities;
    using Enums;
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using System;
    using System.Net.Mail;

    public class SendEmailController : BaseController
    {
        private IFileManager _fileManager;
        private IDomainService<B4.Modules.FileStorage.FileInfo> _fileDomain;

        public SendEmailController(IFileManager fileManager, IDomainService<B4.Modules.FileStorage.FileInfo> fileDomain)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
        }

        public IDomainService<AppealCits> AppealCitsDomain { get; set; }

        public IDomainService<EmailLists> EmailListsDomain { get; set; }

        public IDomainService<AppealCitsAnswer> AppealCitsAnswerDomain { get; set; }

        public ActionResult SendEmail(BaseParams baseParams, Int64 taskId)
        {

            var answer = AppealCitsAnswerDomain.Get(taskId);
            var cits = AppealCitsDomain.Get(answer.AppealCits.Id);

            if (answer == null || cits == null)
                return JsFailure("answer == null || cits == null");

            EmailSender emailSender = EmailSender.Instance;
            emailSender.Send(cits.Email, "Ответ на обращение", MakeMessageBody(cits), MakeAttachment(answer.File));
            EmailLists email = new EmailLists();
            email.AnswerNumber = answer.DocumentNumber;
            email.AppealDate = cits.DateFrom.HasValue? cits.DateFrom.Value : DateTime.MinValue;
            email.AppealNumber = cits.NumberGji;
            if(answer.File != null)
            email.FileInfo = answer.File;
            email.MailTo = cits.Email;
            email.SendDate = DateTime.Now;
            email.Title = "Ответ на обращение";
            if (!string.IsNullOrEmpty(cits.Email))
            {
                EmailListsDomain.Save(email);
            }
            return JsSuccess();
        }

        Attachment MakeAttachment(FileInfo fileInfo)
        {
            if (fileInfo == null)
                return null;

            return new Attachment(_fileManager.GetFile(fileInfo), fileInfo.FullName);
        }

        string MakeMessageBody(AppealCits cits)
        {
            string state = "";
          

            string body = $"Уважаемый(ая) {cits.Correspondent}\r\n";
            body += $"Главное управление Государственная жилищная инспекция  Челябинской области сообщает Вам, что обращение {cits.NumberGji} рассмотрено, файл ответа прикреплен к настоящему электронному сообщению.\r\n";
            body += $"Адрес og@gzhi.gov74.ru используется для автоматического уведомления граждан о результатах рассмотрения обращений поступивших в инспекцию в рамках 59-ФЗ и не предназначен для приема какого-либо рода электронных сообщений (обращений)";
            return body;
        }
    }
}
