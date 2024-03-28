namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
{
    using Entities;  
    using System;
    using System.Linq;
    using System.Web;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Castle.Windsor;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using MailKit.Net.Imap;
    using MailKit.Search;
    using MailKit.Security;
    using MailKit;
    using System.Collections.Generic;
    using Bars.GkhGji.Entities.Email;
    using System.Text;
    using System.Web.Services.Description;
    using MimeKit;
    using System.IO;
    using Bars.Gkh.Report;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.StimulReport;

    public class AppCitOperationsService : IAppCitOperationsService
    {
        public IWindsorContainer Container { get; set; }
        public IFileManager FileManager { get; set; }

        public IDomainService<AppealCits> AppealCitsDomain { get; set; }

        public IDomainService<AppealCitsRealityObject> AppealCitsRealityObjectDomain { get; set; }

        public IDomainService<AppealCitsStatSubject> AppealCitsStatSubjectDomain { get; set; }

        public IDomainService<AppealCitsSource> AppealCitsSourceDomain { get; set; }

        public IDomainService<EmailGji> EmailGjiDomain { get; set; }
        public IDomainService<EmailGjiAttachment> EmailGjiAttachmentDomain { get; set; }
        public IDomainService<EmailGjiLongText> EmailGjiLongTextDomain { get; set; }
        public IDomainService<AppealCitsExecutant> AppealCitsExecutantDomain { get; set; }

        public IDataResult CopyAppeal(BaseParams baseParams)
        {
            var appealId = baseParams.Params.ContainsKey("docId") ? baseParams.Params["docId"].ToLong() : 0;
            if (appealId > 0)
            {
                try
                {
                    var userManager = Container.Resolve<IGkhUserManager>();
                    Operator currentOperator = userManager.GetActiveOperator();
                    Inspector inspector = null;
                    if (currentOperator.Inspector != null)
                        inspector = currentOperator.Inspector;
                    var oldAppeal = AppealCitsDomain.Get(appealId);
                    var newAppeal = new AppealCits
                    {
                       Accepting = oldAppeal.Accepting,
                       AppealRegistrator = inspector != null? inspector:oldAppeal.AppealRegistrator,
                       ApprovalContragent = oldAppeal.ApprovalContragent,
                       CheckTime = oldAppeal.CheckTime,
                       ContragentCorrespondent = oldAppeal.ContragentCorrespondent,
                       Correspondent = oldAppeal.Correspondent,
                       CorrespondentAddress = oldAppeal.CorrespondentAddress,
                       Comment = oldAppeal.Comment,
                       Description = oldAppeal.Description,
                       DateFrom = oldAppeal.DateFrom,
                       Email = oldAppeal.Email,
                       Executant = oldAppeal.Executant,
                       ExtensTime = oldAppeal.ExtensTime,
                       IncomingSources = oldAppeal.IncomingSources,
                       IncomingSourcesName = oldAppeal.IncomingSourcesName,
                       KindStatement = oldAppeal.KindStatement,
                       ManagingOrganization = oldAppeal.ManagingOrganization,
                       Municipality = oldAppeal.Municipality,
                       MunicipalityId = oldAppeal.MunicipalityId,
                       Phone = oldAppeal.Phone,
                       PlannedExecDate = oldAppeal.PlannedExecDate,
                       OrderContragent = oldAppeal.OrderContragent,
                       QuestionStatus = oldAppeal.QuestionStatus,
                       RealityAddresses = oldAppeal.RealityAddresses,
                       SpecialControl = oldAppeal.SpecialControl,
                       SSTUTransferOrg = oldAppeal.SSTUTransferOrg,
                       Surety = oldAppeal.Surety,
                       SuretyDate = oldAppeal.SuretyDate,
                       SuretyResolve = oldAppeal.SuretyResolve,
                       StatementSubjects = oldAppeal.StatementSubjects,
                       TypeCorrespondent = oldAppeal.TypeCorrespondent,
                       Year = oldAppeal.Year,
                       ZonalInspection = oldAppeal.ZonalInspection
                    };
                    AppealCitsDomain.Save(newAppeal);
                    var placeOfOrigin = AppealCitsRealityObjectDomain.GetAll()
                        .Where(x => x.AppealCits.Id == appealId).ToList();
                    foreach (var place in placeOfOrigin)
                    {
                        var newPlace = new AppealCitsRealityObject
                        {
                            AppealCits = newAppeal,
                            RealityObject = place.RealityObject
                        };
                        AppealCitsRealityObjectDomain.Save(newPlace);
                    }
                    var statSubjects = AppealCitsStatSubjectDomain.GetAll()
                       .Where(x => x.AppealCits.Id == appealId).ToList();
                    foreach (var stsub in statSubjects)
                    {
                        var newstsub = new AppealCitsStatSubject
                        {
                            AppealCits = newAppeal,
                            Feature = stsub.Feature,
                            Subject = stsub.Subject,
                            Subsubject = stsub.Subsubject
                        };
                        AppealCitsStatSubjectDomain.Save(newstsub);
                    }

                    var sources = AppealCitsSourceDomain.GetAll()
                  .Where(x => x.AppealCits.Id == appealId).ToList();
                    foreach (var source in sources)
                    {
                        var newsource = new AppealCitsSource
                        {
                            AppealCits = newAppeal,
                           RevenueDate = source.RevenueDate,
                           RevenueForm = source.RevenueForm,
                           RevenueSource = source.RevenueSource,
                           RevenueSourceNumber = source.RevenueSourceNumber
                        };
                        AppealCitsSourceDomain.Save(newsource);
                    }

                    //Проверяющих копировать не требуется
              //      var executants  = AppealCitsExecutantDomain.GetAll()
              //.Where(x => x.AppealCits.Id == appealId).ToList();
              //      foreach (var executant in executants)
              //      {
              //          var newexecutant = new AppealCitsExecutant
              //          {
              //              AppealCits = newAppeal,
              //            Controller = executant.Controller,
              //            Author = executant.Author,
              //            Description = executant.Description,
              //            Executant = executant.Executant,
              //            PerformanceDate = executant.PerformanceDate,
              //            Resolution = executant.Resolution,
              //            State = executant.State,
              //            ZonalInspection = executant.ZonalInspection
              //          };
              //          AppealCitsExecutantDomain.Save(newexecutant);
              //      }
                    return new BaseDataResult { Success = true, Data = newAppeal.Id };
                }
                catch
                {
                    return new BaseDataResult { Success = false, Message = "Не удалось создать копию обращения" };
                }
            }
            else
            {
                return new BaseDataResult { Success = false, Message = "Не найдено обращение для копирования" };
            }
        }

        public IDataResult SyncEmailGJI(BaseParams baseParams)
        {
            DownloadMessages();
            return new BaseDataResult(true, $"Получение почты завершено. Получено: ");
        }

        private  void DownloadMessages()
        {
            using (var client = new ImapClient())
            {
                client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                client.Connect("mail2.gov74.ru", 993, SecureSocketOptions.Auto);
                client.Authenticate("og@gzhi.gov74.ru", "ySp4mXJ0");
                client.Inbox.Open(FolderAccess.ReadWrite);

                var uids = client.Inbox.Search(SearchQuery.NotSeen);
                List<UniqueId> acceptedUUID = new List<UniqueId>();
                int i = 0;
                foreach (var uid in uids)
                {
                    acceptedUUID.Add(uid);
                    var message = client.Inbox.GetMessage(uid);
                    try
                    {
                        var mtext = message.GetTextBody(MimeKit.Text.TextFormat.Text);
                        if (mtext == null)
                        {
                            CreateEmailEntityHTML(message);
                        }
                        else
                        {
                            CreateEmailEntityText(message);
                        }
                        
                    }
                    catch (Exception e)
                    {
                        
                    }

                    i++;
                    if (i > 20)
                        break;
                    // write the message to a file
                    //message.WriteTo(string.Format("{0}.eml", uid));
                }
               
                client.Inbox.AddFlags(acceptedUUID, MessageFlags.Seen, true);

               
                client.Disconnect(true);
            }
        }

        private void CreateEmailEntityHTML(MimeKit.MimeMessage message)
        {
            var htmlB = message.HtmlBody;

            string fromName = string.Empty;
            string fromAddress = string.Empty;
            string appealText = string.Empty;
            string appealTheme = string.Empty;

            foreach (var mailbox in message.From.Mailboxes)
            {
                fromName = mailbox.Name;
                fromAddress = mailbox.Address;
            }
            if (fromAddress == "sus@gov74.ru")
            {
                fromName = GetName(htmlB);
                fromAddress = GetEmail(htmlB);
                appealText = GetAppealText(htmlB);
                appealTheme = GetAppealTheme(htmlB);

            }
            else
            {
                appealTheme = message.Subject;
            }           
            EmailGji newEntity = new EmailGji
            {
                From = fromAddress,
                SenderInfo = fromName,
                Theme = appealTheme,
                GjiNumber = message.MessageId,
                EmailType = GkhGji.Enums.EmailGjiType.NotSet,
                EmailGjiSource = GkhGji.Enums.EmailGjiSource.Email,
                EmailDate = message.Date.Date,
                Registred = false
            };
            EmailGjiDomain.Save(newEntity);
            EmailGjiLongTextDomain.Save(new EmailGjiLongText
            {
                EmailGji = newEntity,
                Content = !string.IsNullOrEmpty(appealText)? Encoding.UTF8.GetBytes(appealText): !string.IsNullOrEmpty(htmlB)? Encoding.UTF8.GetBytes(htmlB): Encoding.UTF8.GetBytes("Нет данных")
            });
            var attachments = message.Attachments.ToList();

            foreach (var attachment in attachments)
            {
                string fileName = string.Empty;
                using (var memory = new MemoryStream())
                {
                    if (attachment is MimeKit.MimePart)
                    {
                        ((MimeKit.MimePart)attachment).Content.DecodeTo(memory);
                        var att = (MimeKit.MimePart)attachment;
                        var fileExt = GetNameAndExtention(att.FileName);
                        var bytes = memory.ToArray();
                        var fileInfo = FileManager.SaveFile(new FileData(fileExt[0], fileExt[1], bytes));
                        if (fileInfo != null)
                        {
                            EmailGjiAttachmentDomain.Save(new EmailGjiAttachment
                            {
                                Message = newEntity,
                                AttachmentFile = fileInfo
                            });
                        }
                    }
                    else
                    {
                        ((MimeKit.MessagePart)attachment).Message.WriteTo(memory);
                        var att = (MimeKit.MessagePart)attachment;
                        //хз че пока с этим делать
                    }

                }
            }
            var fileManager = this.Container.Resolve<IFileManager>();
            try
            {
                var gkhBaseReportDomain = this.Container.ResolveAll<IGkhBaseReport>();
                var report = gkhBaseReportDomain.FirstOrDefault(x => x.Id == "EmailGji");
                var userParam = new UserParamsValues();
                userParam.AddValue("Id", newEntity.Id);
                report.SetUserParams(userParam);
                MemoryStream stream;
                var reportProvider = Container.Resolve<IGkhReportProvider>();
                if (report is IReportGenerator && report.GetType().IsSubclassOf(typeof(StimulReport)))
                {
                    //Вот такой вот костыльный этот метод Все над опеределывать
                    stream = (report as StimulReport).GetGeneratedReport();
                }
                else
                {
                    var reportParams = new ReportParams();
                    report.PrepareReport(reportParams);

                    // получаем Генератор отчета
                    var generatorName = report.GetReportGenerator();

                    stream = new MemoryStream();
                    var generator = Container.Resolve<IReportGenerator>(generatorName);
                    reportProvider.GenerateReport(report, stream, generator, reportParams);

                }
                var file = fileManager.SaveFile(stream, "Входящее письмо PDF.pdf");
                newEntity.EmailPdf = file;
                EmailGjiDomain.Update(newEntity);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                this.Container.Release(fileManager);
            }
        }
        private void CreateEmailEntityText(MimeKit.MimeMessage message)
        {
            var mtext = message.GetTextBody(MimeKit.Text.TextFormat.Text);          
            
            string fromName = string.Empty;
            string fromAddress = string.Empty;
            foreach (var mailbox in message.From.Mailboxes)
            {
                fromName = mailbox.Name;
                fromAddress = mailbox.Address;
            }
            if (fromAddress == "sus@gov74.ru")
            {
                
            }    
            var theme = message.Subject;
            EmailGji newEntity = new EmailGji 
            {
                From = fromAddress,
                SenderInfo = fromName,
                Theme = theme,
                GjiNumber = message.MessageId,
                EmailType = GkhGji.Enums.EmailGjiType.NotSet,
                EmailGjiSource = GkhGji.Enums.EmailGjiSource.Email,
                EmailDate = message.Date.Date,
                Registred = false
            };
            EmailGjiDomain.Save(newEntity);
            EmailGjiLongTextDomain.Save(new EmailGjiLongText
            {
                EmailGji = newEntity,
                Content = !string.IsNullOrEmpty(mtext)? Encoding.UTF8.GetBytes(mtext): Encoding.UTF8.GetBytes("Нет данных")
            });
            var attachments = message.Attachments.ToList();

            foreach (var attachment in attachments)
            {
                string fileName = string.Empty;
                using (var memory = new MemoryStream())
                {
                    if (attachment is MimeKit.MimePart)
                    {
                        ((MimeKit.MimePart)attachment).Content.DecodeTo(memory);
                        var att = (MimeKit.MimePart)attachment;                      
                        var fileExt = GetNameAndExtention(att.FileName);
                        var bytes = memory.ToArray();
                        var fileInfo = FileManager.SaveFile(new FileData(fileExt[0], fileExt[1], bytes));
                        if (fileInfo != null)
                        {
                            EmailGjiAttachmentDomain.Save(new EmailGjiAttachment
                            {
                                Message = newEntity,
                                AttachmentFile = fileInfo
                            });
                        }
                    }                        
                    else
                    {
                        ((MimeKit.MessagePart)attachment).Message.WriteTo(memory);
                        var att = (MimeKit.MessagePart)attachment;
              //хз че пока с этим делать
                    }
                        

                  

                   

                    
                }
            }
            var fileManager = this.Container.Resolve<IFileManager>();
            try
            {
                var gkhBaseReportDomain = this.Container.ResolveAll<IGkhBaseReport>();
                var report = gkhBaseReportDomain.FirstOrDefault(x => x.Id == "EmailGji");
                var userParam = new UserParamsValues();
                userParam.AddValue("Id", newEntity.Id);
                report.SetUserParams(userParam);
                MemoryStream stream;
                var reportProvider = Container.Resolve<IGkhReportProvider>();
                if (report is IReportGenerator && report.GetType().IsSubclassOf(typeof(StimulReport)))
                {
                    //Вот такой вот костыльный этот метод Все над опеределывать
                    stream = (report as StimulReport).GetGeneratedReport();
                }
                else
                {
                    var reportParams = new ReportParams();
                    report.PrepareReport(reportParams);

                    // получаем Генератор отчета
                    var generatorName = report.GetReportGenerator();

                    stream = new MemoryStream();
                    var generator = Container.Resolve<IReportGenerator>(generatorName);
                    reportProvider.GenerateReport(report, stream, generator, reportParams);
                   
                }
                var file = fileManager.SaveFile(stream, "Входящее письмо PDF.pdf");
                newEntity.EmailPdf = file;
                EmailGjiDomain.Update(newEntity);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                this.Container.Release(fileManager);
            }
        }

        private string[] GetNameAndExtention(string fullFileName)
        {
            var result = new string[2];

            var splittedName = fullFileName.Split('.');

            result[1] = splittedName[splittedName.Length - 1];

            var resultName = new StringBuilder();

            for (var i = 0; i < splittedName.Length - 1; i++)
            {
                resultName.Append(string.Format("{0}.", splittedName[i]));
            }

            resultName.Remove(resultName.Length - 1, 1);

            result[0] = resultName.ToString();

            return result;
        }

        private string GetAppealTheme(string body)
        {
            string res = string.Empty;
            body = body.Replace("<br />", "");
            body = body.Replace("</b>", "");
            if (body.Split(new string[] { "<b>Тема: " }, StringSplitOptions.None).Length > 1)
            {
                string fam = body.Split(new string[] { "<b>Тема: " }, StringSplitOptions.None)[1];
                {
                    if (fam.Split(new string[] { "<b>Текст обращения:" }, StringSplitOptions.None).Length > 1)
                    {
                        res = res + fam.Split(new string[] { "<b>Текст обращения:" }, StringSplitOptions.None)[0];
                        res = res.Replace("\r\n", " ");
                    }
                }
            }

            return res.Trim();
        }

        private string GetAppealText(string body)
        {
            string res = string.Empty;
            body = body.Replace("<br />", "");
            body = body.Replace("</b>", "");
            if (body.Split(new string[] { "Текст обращения: " }, StringSplitOptions.None).Length > 1)
            {
                string fam = body.Split(new string[] { "Текст обращения: " }, StringSplitOptions.None)[1];
                {
                    if (fam.Split(new string[] { "<b>Ваш E-mail: " }, StringSplitOptions.None).Length > 1)
                    {
                        res = res + fam.Split(new string[] { "<b>Ваш E-mail: " }, StringSplitOptions.None)[0];
                        res = res.Replace("\r\n", " ");
                    }
                }
            }

            return res.Trim();
        }

        private string GetEmail(string body)
        {
            string res = string.Empty;
            body = body.Replace("<br />", "");
            body = body.Replace("</b>", "");
            if (body.Split(new string[] { "<b>Ваш E-mail: " }, StringSplitOptions.None).Length > 1)
            {
                string fam = body.Split(new string[] { "<b>Ваш E-mail: " }, StringSplitOptions.None)[1];
                {
                    if (fam.Split(new string[] { "<b>Ваш телефон:" }, StringSplitOptions.None).Length > 1)
                    {
                        res = res + fam.Split(new string[] { "<b>Ваш телефон:" }, StringSplitOptions.None)[0];
                        res = res.Replace("\r\n", " ");
                    }
                }
            }

            return res.Trim();
        }

        private string GetName(string body)
        {
            string res = string.Empty;
            body = body.Replace("<br />", "");
            body = body.Replace("</b>", "");
            if (body.Split(new string[] { "<b>Фамилия: " }, StringSplitOptions.None).Length > 1)
            {
                string fam = body.Split(new string[] { "<b>Фамилия: " }, StringSplitOptions.None)[1];
                {
                    if (fam.Split(new string[] { "<b>Имя: " }, StringSplitOptions.None).Length > 1)
                    {
                        res = res + fam.Split(new string[] { "<b>Имя: " }, StringSplitOptions.None)[0];
                        res = res.Replace("\r\n", " ");
                    }
                }
            }
            if (body.Split(new string[] { "<b>Имя: " }, StringSplitOptions.None).Length > 1)
            {
                string fam = body.Split(new string[] { "<b>Имя: " }, StringSplitOptions.None)[1];
                {
                    if (fam.Split(new string[] { "<b>Отчество: " }, StringSplitOptions.None).Length > 1)
                    {
                        res = res + fam.Split(new string[] { "<b>Отчество: " }, StringSplitOptions.None)[0];
                        res = res.Replace("\r\n", " ");
                    }
                }
            }
            if (body.Split(new string[] { "<b>Отчество: " }, StringSplitOptions.None).Length > 1)
            {
                string fam = body.Split(new string[] { "<b>Отчество: " }, StringSplitOptions.None)[1];
                {
                    if (fam.Split(new string[] { "<b>Тема: " }, StringSplitOptions.None).Length > 1)
                    {
                        res = res + fam.Split(new string[] { "<b>Тема: " }, StringSplitOptions.None)[0];
                        res = res.Replace("\r\n", " ");
                    }
                }
            }


            return res.Trim();
        }



    }
}