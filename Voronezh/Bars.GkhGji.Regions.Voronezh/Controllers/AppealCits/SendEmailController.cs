namespace Bars.GkhGji.Regions.Voronezh.Controllers
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
            email.AnswerDate = answer.DocumentDate;
            email.GjiNumber = cits.DocumentNumber;
            if (answer.File != null)
            email.FileInfo = answer.File;
            email.MailTo = cits.Email;
            email.SendDate = DateTime.Now;
            email.Title = "Ответ на обращение";
            if (answer.Executor != null)
            {
                email.Executor = answer.Executor.Fio;
            }
            if (!string.IsNullOrEmpty(cits.Email))
            {
                EmailListsDomain.Save(email);
            }
            return JsSuccess();
        }

        public ActionResult SendEmailFKR(BaseParams baseParams, Int64 taskId)
        {

            var answer = AppealCitsAnswerDomain.Get(taskId);
            var cits = AppealCitsDomain.Get(answer.AppealCits.Id);

            if (answer.State == null)
            {
                return JsFailure("Отправка ответа на данном статусе запрещена");
            }
            if (!answer.State.FinalState)
            {
                return JsFailure("Отправка ответа на данном статусе запрещена");
            }

            if (answer == null || cits == null)
                return JsFailure("answer == null || cits == null");

            EmailSender emailSender = EmailSender.Instance;
            emailSender.SendFKR(cits.Email, "Ответ на обращение", MakeMessageBodyFKR(cits, answer), MakeAttachment(answer.File));
            EmailLists email = new EmailLists();
            email.AnswerNumber = answer.DocumentNumber;
            email.AppealDate = cits.DateFrom.HasValue ? cits.DateFrom.Value : DateTime.MinValue;
            email.AppealNumber = cits.NumberGji;
            email.AnswerDate = answer.DocumentDate;
            email.GjiNumber = cits.DocumentNumber;
            if (answer.File != null)
                email.FileInfo = answer.File;
            email.MailTo = cits.Email;
            email.SendDate = DateTime.Now;
            email.Title = "Ответ на обращение";
            if (answer.Executor != null)
            {
                email.Executor = answer.Executor.Fio;
            }
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
          

            string body = $"Уважаемый(ая) {cits.Correspondent}!\r\n";
            body += $"Государственная жилищная инспекция Воронежской области сообщает, что обращение от {cits.DateFrom.Value.ToShortDateString()} № {cits.DocumentNumber} рассмотрено, файл ответа прикреплен к настоящему электронному сообщению.\r\n";
            body += $"Данное электронное сообщение сформировано автоматически и не требует ответа.\r\n";
            body += $"Адрес gzhi@govvrn.ru используется для автоматического уведомления граждан о результатах рассмотрения обращений, поступивших в инспекцию, и не предназначен для приема электронных сообщений (обращений)";
            return body;
        }

        string MakeMessageBodyFKR(AppealCits cits, AppealCitsAnswer answer)
        {
            string state = "";


            string body = $"Уважаемый(ая) {cits.Correspondent}!\r\n";
            if (answer.File != null)
            {
                body += $"Фонд капитального ремонта Воронежской  области сообщает, что обращение от {cits.DateFrom.Value.ToShortDateString()} № {cits.DocumentNumber} рассмотрено, файл ответа прикреплен к настоящему электронному сообщению.\r\n";
                if (answer.AnswerContent != null)
                {
                    body += $"{answer.AnswerContent.Name}.\r\n";
                }
            }
            else if (answer.AnswerContent != null)
            {
                body += $"Фонд капитального ремонта Воронежской  области сообщает, что обращение от {cits.DateFrom.Value.ToShortDateString()} № {cits.DocumentNumber} рассмотрено.\r\n";
                body += $"{answer.AnswerContent.Name}.\r\n";
            }
            else
            {
                body += $"Фонд капитального ремонта Воронежской  области сообщает, что обращение от {cits.DateFrom.Value.ToShortDateString()} № {cits.DocumentNumber} рассмотрено.\r\n";
            }
            body += $"Данное электронное сообщение сформировано автоматически и не требует ответа.\r\n";
            body += $"Адрес info@fkr36.ru используется для автоматического уведомления граждан о результатах рассмотрения обращений, поступивших в инспекцию, и не предназначен для приема электронных сообщений (обращений)";
            return body;
        }
    }
}
