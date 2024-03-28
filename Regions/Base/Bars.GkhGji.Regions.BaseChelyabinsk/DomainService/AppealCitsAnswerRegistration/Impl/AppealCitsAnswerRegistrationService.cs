namespace Bars.GkhGji.DomainService.Impl
{
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Modules.Tasks.Common.Utils;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Email;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
    using Bars.GkhGji.Tasks;
    using Castle.Windsor;
    using System;
    using System.Linq;
    using System.Net.Mail;
    using System.Net;

    public class AppealCitsAnswerRegistrationService : IAppealCitsAnswerRegistrationService
    {
        public IWindsorContainer Container { get; set; }
        public IGkhUserManager UserManager { get; set; }
        public IFileManager FileManager { get; set; }
        public IDomainService<AppealCitsAnswer> AppealCitsAnswerDomain { get; set; }
        public IDomainService<AppealAnswerLongText> AppealAnswerLongTextDomain { get; set; }

        public IDataResult GetList(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = AppealCitsAnswerDomain.GetAll()
                .Where(x => x.SignedFile != null)
                .Where(x => x.TypeAppealFinalAnswer == TypeAppealFinalAnswer.Answer)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentName,
                    AppealState = x.AppealCits.State.Name,
                    AppealNumber = $"{x.AppealCits.Number}({x.AppealCits.NumberGji})",
                    DocumentDate = x.DocumentDate != DateTime.MinValue ? x.DocumentDate : null,
                    x.DocumentNumber,
                    x.SignedFile,
                    x.TypeAppealAnswer,
                    x.AppealCits.Correspondent,
                    SignerFio = x.Signer != null 
                                ? x.Signer.Fio
                                    : x.Executor != null
                                    ? x.Executor.Fio
                                :"",
                    x.Sended,
                    x.SendedToEdm,
                    x.Registred
                })
                .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }

        public IDataResult GetAnswer(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            var answer = AppealCitsAnswerDomain.Get(id);

            if (answer != null)
            {
                return new BaseDataResult(
                    new
                    {
                        answer.Id,
                        TypeAppealFinalAnswer = answer.TypeAppealFinalAnswer.GetDisplayName(),
                        answer.DocumentName,
                        answer.DocumentNumber,
                        answer.DocumentDate,
                        Addressee = answer.Addressee.Name,
                        RedirectContragent = answer.RedirectContragent.Name,
                        Executor = answer.Executor.Fio,
                        Signer = answer.Signer.Fio,
                        AnswerContent = answer.AnswerContent.Name,
                        answer.File,
                        answer.SignedFile,
                        answer.ExecDate,
                        answer.ExtendDate,
                        FactCheckingType = answer.FactCheckingType.Name,
                        ConcederationResult = answer.ConcederationResult.Name,
                        answer.SendedToEdm,
                    });
            }

            return new BaseDataResult();
        }

        public IDataResult GetNextQuestion(BaseParams baseParams)
        {
            try
            {
                var answer = AppealCitsAnswerDomain.GetAll()
                    .Where(x => !x.Registred && x.SignedFile != null)
                    .OrderBy(x => x.Id)
                    .FirstOrDefault();

                if (answer == null)
                {
                    return new BaseDataResult(new
                    {
                        QlistName = "",
                        Question = "",
                        qid = 0
                    });
                }

                return new BaseDataResult(new
                {
                    FileId = answer.SignedFile.Id,
                    AnswerInfo = $"{answer.AppealCits.Number}({answer.AppealCits.NumberGji})",
                    Email = answer.AppealCits.Email,
                    AnswerId = answer.Id,
                });
            }
            catch (Exception e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }
        public IDataResult SkipAndGetNextQuestion(BaseParams baseParams)
        {
            var answerId = baseParams.Params.ContainsKey("answerId")
                                     ? baseParams.Params["answerId"].ToLong()
                                     : 0;

            try
            {
                var answer = AppealCitsAnswerDomain.GetAll()
                    .Where(x => !x.Registred && x.SignedFile != null && x.Id > answerId)
                    .OrderBy(x => x.Id)
                    .FirstOrDefault();

                if (answer == null)
                {
                    return new BaseDataResult(new
                    {
                        QlistName = "",
                        Question = "",
                        qid = 0
                    });
                }

                return new BaseDataResult(new
                {
                    FileId = answer.SignedFile.Id,
                    AnswerInfo = $"{answer.AppealCits.Number}({answer.AppealCits.NumberGji})",
                    Email = answer.AppealCits.Email,
                    AnswerId = answer.Id,
                });
            }
            catch (Exception e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }

        public IDataResult RegisterAnswer(BaseParams baseParams)
        {
            var answerId = baseParams.Params.ContainsKey("answerId")
                                     ? baseParams.Params["answerId"].ToLong()
                                     : 0;

            var number = baseParams.Params.GetAs<long>("number");
            var pref = baseParams.Params.GetAs<string>("pref");
            var suf = baseParams.Params.GetAs<string>("suf");

            try
            {
                var answer = AppealCitsAnswerDomain.Get(answerId);
                if (answer == null)
                {
                    return new BaseDataResult(new
                    {
                        QlistName = "",
                        Question = "",
                        qid = 0
                    });
                }

                Operator thisOperator = UserManager.GetActiveOperator();
                if (thisOperator?.Inspector == null)
                {
                    return new BaseDataResult(new
                    {
                        QlistName = "",
                        Question = "",
                        qid = 0
                    });
                }

                answer.DocumentNumber = GetAppealNumber(pref, number, suf);
                answer.DocumentDate = answer.DocumentDate ?? DateTime.Now;
                answer.ExecDate = answer.ExecDate ?? DateTime.Now;
                answer.Registred = true;
                AppealCitsAnswerDomain.Update(answer);

                return new BaseDataResult(new
                {
                    FileId = answer.SignedFile.Id,
                    AnswerInfo = $"{answer.AppealCits.Number}({answer.AppealCits.NumberGji})",
                    Email = answer.AppealCits.Email,
                    AnswerId = answer.Id,
                });
            }
            catch (Exception e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }

        public IDataResult RegisterAndSendAnswer(BaseParams baseParams)
        {
            var answerId = baseParams.Params.ContainsKey("answerId")
                                     ? baseParams.Params["answerId"].ToLong()
                                     : 0;

            var number = baseParams.Params.GetAs<long>("number");
            var pref = baseParams.Params.GetAs<string>("pref");
            var suf = baseParams.Params.GetAs<string>("suf");

            try
            {
                var answer = AppealCitsAnswerDomain.Get(answerId);
                if (answer == null)
                {
                    return new BaseDataResult(new
                    {
                        QlistName = "",
                        Question = "",
                        qid = 0
                    });
                }

                Operator thisOperator = UserManager.GetActiveOperator();
                if (thisOperator?.Inspector == null)
                {
                    return new BaseDataResult(new
                    {
                        QlistName = "",
                        Question = "",
                        qid = 0
                    });
                }

                answer.DocumentNumber = GetAppealNumber(pref, number, suf);
                answer.DocumentDate = answer.DocumentDate ?? DateTime.Now;
                answer.ExecDate = answer.ExecDate ?? DateTime.Now;
                answer.Registred = true;
                
                var sendSuccess = SendEmail(answer);

                if (sendSuccess)
                {
                    answer.Sended = true;
                }

                AppealCitsAnswerDomain.Update(answer);

                return new BaseDataResult(new
                {
                    FileId = answer.SignedFile.Id,
                    AnswerInfo = $"{answer.AppealCits.Number}({answer.AppealCits.NumberGji})",
                    Email = answer.AppealCits.Email,
                    AnswerId = answer.Id,
                });
            }
            catch (Exception e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }

        private string GetAppealNumber(string appPref, long appNum, string appSuf)
        {

            if (!string.IsNullOrEmpty(appPref))
            {
                return $"{appPref}{appNum}{appSuf}";
            }
            else
            {
                return $"{appNum}{appSuf}";
            }
        }

        private bool SendEmail(AppealCitsAnswer answer)
        {
            var appSettings = ApplicationContext.Current.Configuration.AppSettings;
            var smtpClient = appSettings.GetAs<string>("smtpClient");
            var smtpPort = appSettings.GetAs<int>("smtpPort");
            var smtpEmail = appSettings.GetAs<string>("smtpEmail");
            var smtpLogin = appSettings.GetAs<string>("smtpLogin");
            var smtpPassword = appSettings.GetAs<string>("smtpPassword");

            var answerLongText = AppealAnswerLongTextDomain.GetAll()
                    .Where(x => x.AppealCitsAnswer.Id == answer.Id)
                    .FirstOrDefault();

            string messageBody = string.Empty;
            if (answerLongText != null)
            {
                messageBody = System.Text.Encoding.UTF8.GetString(answerLongText.Description2);
            }

            var success = false;
            try
            {
                using (var client = new SmtpClient(smtpClient, smtpPort))
                using (var message = new MailMessage(smtpEmail, answer.AppealCits.Email, "Ответ по обращению", messageBody))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(smtpLogin, smtpPassword);

                    var attachStream = FileManager.GetFile(answer.SignedFile);
                    attachStream.Position = 0;

                    using (attachStream)
                    using (var attachment = new Attachment(attachStream, answer.SignedFile.FullName, "application/pdf"))
                    {
                        message.Attachments.Add(attachment);

                        try
                        {
                            client.Send(message);
                            success = true;
                        }
                        catch
                        {
                            try
                            {
                                client.EnableSsl = false;
                                client.Send(message);
                                success = true;
                            }
                            catch (Exception)
                            {

                            }
                        }

                    }
                }
            }
            catch (Exception)
            {

            }

            return success;
        }
    }
}