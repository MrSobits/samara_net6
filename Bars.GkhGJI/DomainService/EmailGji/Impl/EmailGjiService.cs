namespace Bars.GkhGji.DomainService.Impl
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Report;
    using Bars.Gkh.StimulReport;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Entities.Email;
    using Bars.GkhGji.Enums;
    using Castle.Windsor;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using MailKit.Net.Imap;
    using MailKit.Search;
    using MailKit.Security;
    using MailKit;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.B4.Modules.Tasks.Common.Service;
    using System.Threading.Tasks;
    using Bars.GkhGji.Tasks;
    using Bars.Gkh.Services.DataContracts.GetMainInfoManOrg;

    public class EmailGjiService : IEmailGjiService
    {
        public IWindsorContainer Container { get; set; }
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<EmailGji> EmailGjiDomain { get; set; }
        public IDomainService<EmailGjiLongText> EmailGjiLongTextDomain { get; set; }
        public IDomainService<AppealCits> AppealCitsDomain { get; set; }
        public IDomainService<AppealCitsRealityObject> AppealCitsRealityObjectDomain { get; set; }
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }
        public IDomainService<RevenueSourceGji> RevenueSourceGjiDomain { get; set; }
        public IDomainService<RevenueFormGji> RevenueFormGjiDomain { get; set; }
        public IDomainService<KindStatementGji> KindStatementGjiDomain { get; set; }
        public IDomainService<AppealCitsSource> AppealCitsSourceDomain { get; set; }
        public IDomainService<AppealCitsAttachment> AppealCitsAttachmentDomain { get; set; }
        public IDomainService<EmailGjiAttachment> EmailGjiAttachmentDomain { get; set; }
        public IDataResult PrintReport(BaseParams baseParams)
        {
            var serviceActCheckAnnex = this.Container.Resolve<IDomainService<ActCheckAnnex>>();     

            try
            {
                var documentId = baseParams.Params.ContainsKey("documentId")
                                     ? baseParams.Params["documentId"].ToLong()
                                     : 0;
                var userParam = new UserParamsValues();
                userParam.AddValue("DocumentId", documentId);
                var codedReportDomain = this.Container.ResolveAll<IGkhBaseReport>();
                var report = codedReportDomain.FirstOrDefault(x => x.Id == "ActChectQListAnswer");
                report.SetUserParams(userParam);
                MemoryStream stream;
                var reportProvider = Container.Resolve<IGkhReportProvider>();
                if (report is IReportGenerator && report.GetType().IsSubclassOf(typeof(StimulReport)))
                {
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
                var fileManager = this.Container.Resolve<IFileManager>();
                var reportFile = fileManager.SaveFile(stream, "Проверочный лист.docx");
                if (reportFile != null)
                {
                    var existsReportAnnex = serviceActCheckAnnex.GetAll()
                        .Where(x => x.ActCheck.Id == documentId && x.TypeAnnex == Enums.TypeAnnex.ControlList).FirstOrDefault();
                    if (existsReportAnnex != null)
                    {
                        existsReportAnnex.File = reportFile;
                        serviceActCheckAnnex.Update(existsReportAnnex);
                    }
                    else
                    {
                        serviceActCheckAnnex.Save(new ActCheckAnnex
                        {
                            File = reportFile,
                            ActCheck = new ActCheck {Id = documentId },
                            MessageCheck = Enums.MessageCheck.NotSet,
                            Description = "Сформирован автоматически",
                            DocumentDate = DateTime.Now,
                            Name = "Проверочный лист",
                            TypeAnnex = Enums.TypeAnnex.ControlList

                        });
                    }
                }
                return new BaseDataResult(true);
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                this.Container.Release(serviceActCheckAnnex);

            }
        }
        public IDataResult SaveAndGetNextQuestion(BaseParams baseParams)
        {
           
            return new BaseDataResult(new
            {
                QlistName = "",
                Question = "",
                qid = 0
            });
               
        }     

        public IDataResult GetNextQuestion(BaseParams baseParams)
        {  
            try
            {               
                var email = EmailGjiDomain.GetAll().Where(x => !x.Registred && x.EmailPdf != null).OrderBy(x => x.Id).FirstOrDefault();
                if (email == null)
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
                    FileId = email.EmailPdf.Id,
                    SenderInfo = email.SenderInfo,
                    Theme = email.Theme,
                    EmailId = email.Id,
                });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {}
        }
        public IDataResult SkipAndGetNextQuestion(BaseParams baseParams)
        {
            var documentId = baseParams.Params.ContainsKey("documentId")
                                     ? baseParams.Params["documentId"].ToLong()
                                     : 0;

            if (documentId == 0)
            {
                return new BaseDataResult(new
                {
                    QlistName = "",
                    Question = "",
                    qid = 0
                });
            }

            try
            {
                var email = EmailGjiDomain.GetAll().Where(x => !x.Registred && x.EmailPdf != null && x.Id> documentId).OrderBy(x => x.Id).FirstOrDefault();
                if (email == null)
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
                    FileId = email.EmailPdf.Id,
                    SenderInfo = email.SenderInfo,
                    Theme = email.Theme,
                    EmailId = email.Id,
                });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            { }
        }

        public IDataResult RegisterEmail(BaseParams baseParams)
        {
            var documentId = baseParams.Params.ContainsKey("documentId")
                                     ? baseParams.Params["documentId"].ToLong()
                                     : 0;
            var msgType = baseParams.Params.GetAs<int>("msgType");
            var appNumber = baseParams.Params.GetAs<long>("appNumber");
            var appPref = baseParams.Params.GetAs<string>("appPref");
            var appSuf = baseParams.Params.GetAs<string>("appSuf");

            if (documentId == 0)
            {
                return new BaseDataResult(new
                {
                    QlistName = "",
                    Question = "",
                    qid = 0
                });
            }

            try
            {
                var email = EmailGjiDomain.Get(documentId);
                if (email == null)
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
                    return new BaseDataResult(new
                    {
                        QlistName = "",
                        Question = "",
                        qid = 0
                    });
                if (email.EmailGjiSource == EmailGjiSource.Email)
                {
                    string mailId = email.GjiNumber;
                    email.GjiNumber = GetAppealNumber(appPref, appNumber, appSuf);
                    email.Description = mailId;
                    EmailGjiDomain.Update(email);

                    string descr = string.Empty;
                    try
                    {
                        var emailLT = EmailGjiLongTextDomain.GetAll().FirstOrDefault(x => x.EmailGji == email);
                        if (emailLT != null)
                        {
                            descr = System.Text.Encoding.UTF8.GetString(emailLT.Content);
                            if (descr.Length > 9999)
                            {
                                descr = descr.Substring(0, 9999);
                            }
                        }
                    }
                    catch
                    { }

                    AppealCits newAppeal = new AppealCits
                    {
                        AppealRegistrator = thisOperator.Inspector,
                        AppealStatus = AppealStatus.Uncontrol,
                        CaseDate = DateTime.Now,
                        DateFrom = DateTime.Now,
                        Correspondent = email.SenderInfo,
                        Email = email.From,
                        DocumentNumber = GetAppealNumber(appPref, appNumber, appSuf),
                        IntNumber = Convert.ToInt32(appNumber),
                        File = GetPrintForm(email.Id, GetAppealNumber(appPref, appNumber, appSuf)),
                        KindStatement = KindStatementGjiDomain.GetAll().FirstOrDefault(x => x.Code == "02"),
                        NumberGji = GetAppealNumber(appPref, appNumber, appSuf),
                        Description = !string.IsNullOrEmpty(descr) ? descr : !string.IsNullOrEmpty(email.Theme) ? email.Theme : "Без темы"
                    };

                    using (var transaction = Container.Resolve<IDataTransaction>())
                    {
                        try
                        {
                            AppealCitsDomain.Save(newAppeal);
                            EmailGjiAttachmentDomain.GetAll().Where(x => x.Message == email).ToList().ForEach(x =>
                            {
                                AppealCitsAttachmentDomain.Save(new AppealCitsAttachment
                                {
                                    AppealCits = newAppeal,
                                    FileInfo = x.AttachmentFile,
                                    Name = x.AttachmentFile.Name,
                                });
                            });
                            AppealCitsSourceDomain.Save(new AppealCitsSource
                            {
                                AppealCits = newAppeal,
                                RevenueDate = email.EmailDate,
                                RevenueForm = RevenueFormGjiDomain.GetAll().FirstOrDefault(x => x.Name == "Электронная почта"),
                                RevenueSource = RevenueSourceGjiDomain.GetAll().FirstOrDefault(x => x.Name == "Электронная почта"),
                                RevenueSourceNumber = email.GjiNumber,
                                SSTUDate = email.EmailDate
                            });

                            transaction.Commit();

                        }
                        catch (ValidationException e)
                        {
                            transaction.Rollback();

                        }
                        catch
                        {
                            transaction.Rollback();

                        }
                        finally
                        {

                        }
                    }

                    email.Registred = true;

                    EmailGjiDomain.Update(email);
                    try
                    {
                        if (!baseParams.Params.ContainsKey("taskId"))
                            baseParams.Params.Add("taskId", email.Id);
                        var taskManager = this.Container.Resolve<ITaskManager>();
                        taskManager.CreateTasks(new EmailGjiTaskProvider(Container), baseParams);
                        //using (var client = new ImapClient())
                        //{
                        //    client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                        //    client.Connect("mail2.gov74.ru", 993, SecureSocketOptions.Auto);
                        //    client.Authenticate("og@gzhi.gov74.ru", "ySp4mXJ0");
                        //    client.Inbox.Open(FolderAccess.ReadWrite);

                        //    var declineFolder = client.GetFolder("Зарегистрированные");
                        //    var uids = client.Inbox.Search(SearchQuery.HeaderContains("Message-Id", email.Description));
                        //    if (uids.Count > 0)
                        //    {
                        //        client.Inbox.MoveTo(uids, declineFolder);
                        //    }
                        //    client.Disconnect(true);
                        //}
                    }
                    catch (Exception e)
                    {

                    }

                    return new BaseDataResult(new
                    {
                        FileId = email.EmailPdf.Id,
                        SenderInfo = email.SenderInfo,
                        Theme = email.Theme,
                        EmailId = email.Id,
                    });
                }
                else if (email.EmailGjiSource == EmailGjiSource.POS)
                {
                    string emailDescr = email.Description;
                    string mailId = email.GjiNumber;
                    email.GjiNumber = GetAppealNumber(appPref, appNumber, appSuf);
                    email.Description = mailId;
                    EmailGjiDomain.Update(email);

                    string descr = string.Empty;
                    try
                    {
                        var emailLT = EmailGjiLongTextDomain.GetAll().FirstOrDefault(x => x.EmailGji == email);
                        if (emailLT != null)
                        {
                            descr = System.Text.Encoding.UTF8.GetString(emailLT.Content);
                            if (descr.Length > 9999)
                            {
                                descr = descr.Substring(0, 9999);
                            }
                        }
                    }
                    catch
                    { }
                    
                    AppealCits newAppeal = new AppealCits
                    {
                        AppealRegistrator = thisOperator.Inspector,
                        AppealStatus = AppealStatus.Uncontrol,
                        CaseDate = DateTime.Now,
                        DateFrom = DateTime.Now,
                        Correspondent = email.SenderInfo,
                        Phone = GetTextData(emailDescr, "phone"),
                        CorrespondentAddress = GetTextData(emailDescr, "address"),
                        Email = email.From,
                        DocumentNumber = GetAppealNumber(appPref, appNumber, appSuf),
                        IntNumber = Convert.ToInt32(appNumber),
                        File = GetPOSPrintForm(email.Id, GetAppealNumber(appPref, appNumber, appSuf)),
                        KindStatement = KindStatementGjiDomain.GetAll().FirstOrDefault(x => x.Code == "02"),
                        NumberGji = GetAppealNumber(appPref, appNumber, appSuf),
                        Description = !string.IsNullOrEmpty(descr) ? descr : !string.IsNullOrEmpty(email.Theme) ? email.Theme : "Без темы"
                    };
                    AppealCitsRealityObject newappcitRO = null;
                    var adr = GetTextData(emailDescr, "probplace");
                    if (!string.IsNullOrEmpty(adr))
                    {
                        var realityobj = RealityObjectDomain.GetAll().Where(x => x.Municipality != null)
                       .FirstOrDefault(x => adr.StartsWith(x.Municipality.Name) && adr.EndsWith(x.Address));
                        if (realityobj != null)
                        {
                            newappcitRO = new AppealCitsRealityObject
                            {
                                AppealCits = newAppeal,
                                RealityObject = realityobj
                            };
                        }
                    }
                   

                    using (var transaction = Container.Resolve<IDataTransaction>())
                    {
                        try
                        {
                            AppealCitsDomain.Save(newAppeal);
                            if (newappcitRO != null)
                            {
                                AppealCitsRealityObjectDomain.Save(newappcitRO);
                            }
                            EmailGjiAttachmentDomain.GetAll().Where(x => x.Message == email).ToList().ForEach(x =>
                            {
                                AppealCitsAttachmentDomain.Save(new AppealCitsAttachment
                                {
                                    AppealCits = newAppeal,
                                    FileInfo = x.AttachmentFile,
                                    Name = x.AttachmentFile.Name,
                                });
                            });
                            AppealCitsSourceDomain.Save(new AppealCitsSource
                            {
                                AppealCits = newAppeal,
                                RevenueDate = email.EmailDate,
                                RevenueForm = RevenueFormGjiDomain.GetAll().FirstOrDefault(x => x.Name == "Интернет"),
                                RevenueSource = RevenueSourceGjiDomain.GetAll().FirstOrDefault(x => x.Name == "Платформа обратной связи"),
                                RevenueSourceNumber = email.GjiNumber,
                                SSTUDate = email.EmailDate
                            });

                            transaction.Commit();

                        }
                        catch (ValidationException e)
                        {
                            transaction.Rollback();

                        }
                        catch
                        {
                            transaction.Rollback();

                        }
                        finally
                        {

                        }
                    }

                    email.Registred = true;

                    EmailGjiDomain.Update(email);
                 

                    return new BaseDataResult(new
                    {
                        FileId = email.EmailPdf.Id,
                        SenderInfo = email.SenderInfo,
                        Theme = email.Theme,
                        EmailId = email.Id,
                    });
                }

                return null;
               
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            { }
        }

        public IDataResult DeclineEmail(BaseParams baseParams)
        {
            var documentId = baseParams.Params.ContainsKey("documentId")
                                     ? baseParams.Params["documentId"].ToLong()
                                     : 0;
            var typeDecline = baseParams.Params.GetAs<EmailDenailReason>("typeDecline");
            var declineReason = baseParams.Params.GetAs<string>("declineReason");

            if (documentId == 0)
            {
                return new BaseDataResult(new
                {
                    QlistName = "",
                    Question = "",
                    qid = 0
                });
            }

            try
            {
                var email = EmailGjiDomain.Get(documentId);
                if (email == null)
                {
                    return new BaseDataResult(new
                    {
                        QlistName = "",
                        Question = "",
                        qid = 0
                    });
                }

                email.DeclineReason = declineReason;
                email.Registred = true;
                email.Description = email.GjiNumber;
                email.EmailDenailReason = typeDecline;
                EmailGjiDomain.Update(email);
                try
                {
                    //using (var client = new ImapClient())
                    //{
                    //    client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                    //    client.Connect("mail2.gov74.ru", 993, SecureSocketOptions.Auto);
                    //    client.Authenticate("og@gzhi.gov74.ru", "ySp4mXJ0");
                    //    client.Inbox.Open(FolderAccess.ReadWrite);

                    //    var declineFolder = client.GetFolder("Отклоненные");
                    //    var uids = client.Inbox.Search(SearchQuery.HeaderContains("Message-Id", email.GjiNumber));
                    //    if (uids.Count > 0)
                    //    {
                    //        client.Inbox.MoveTo(uids, declineFolder);
                    //    }
                    //    client.Disconnect(true);
                    //}
                }
                catch (Exception e)
                {

                }

                return new BaseDataResult(new
                {
                    FileId = email.EmailPdf.Id,
                    SenderInfo = email.SenderInfo,
                    Theme = email.Theme,
                    EmailId = email.Id,
                });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            { }
        }

        public IDataResult GetListAttachments(BaseParams baseParams)
        {
            var documentId = baseParams.Params.ContainsKey("documentId")
                                     ? baseParams.Params["documentId"].ToLong()
                                     : 0;
           
            var loadParams = baseParams.GetLoadParam();
            var emailGji = EmailGjiDomain.Get(documentId);

            var data = EmailGjiAttachmentDomain.GetAll()
                .Where(x => x.Message.Id == documentId)
                .Select(x => new
                {
                   x.AttachmentFile.Id,
                   FileName = x.AttachmentFile.Name,
                   x.AttachmentFile
                })              
                .Filter(loadParams, Container).ToList();
            data.Add(new
            {
                Id = emailGji.EmailPdf.Id,
                FileName = emailGji.EmailPdf.Name,
                AttachmentFile = emailGji.EmailPdf
            });

            var totalCount = data.Count;

            return new ListDataResult(data.AsQueryable().Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        private string GetTextData(string descr, string typeStr)
        {
            if (!string.IsNullOrEmpty(descr))
            {
                if (typeStr == "phone")
                {
                    string sep1 = "<b>Телефон:</b> ";
                    string sep2 = " <br><b>Адрес";
                    if (descr.Split(new string[] { sep1 }, StringSplitOptions.None).Length > 1)
                    {
                        string fam = descr.Split(new string[] { sep1 }, StringSplitOptions.None)[1];
                        {
                            if (fam.Split(new string[] { sep2 }, StringSplitOptions.None).Length > 1)
                            {
                               return fam.Split(new string[] { sep2 }, StringSplitOptions.None)[0];
                            }
                        }
                    }
                    return "";
                }
                if (typeStr == "address")
                {
                    string sep1 = "Адрес заявителя:</b> ";
                    string sep2 = "<br><b>Место";
                    if (descr.Split(new string[] { sep1 }, StringSplitOptions.None).Length > 1)
                    {
                        string fam = descr.Split(new string[] { sep1 }, StringSplitOptions.None)[1];
                        {
                            if (fam.Split(new string[] { sep2 }, StringSplitOptions.None).Length > 1)
                            {
                                return fam.Split(new string[] { sep2 }, StringSplitOptions.None)[0];
                            }
                        }
                    }
                    return "";
                }
                if (typeStr == "probplace")
                {
                    string sep1 = "проблемы:</b> ";
                    if (descr.Split(new string[] { sep1 }, StringSplitOptions.None).Length > 1)
                    {
                        string fam = descr.Split(new string[] { sep1 }, StringSplitOptions.None)[1];
                        {
                            return fam;
                        }
                    }
                    return "";
                }
                return "";
            }
            else
            return "";
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

        private B4.Modules.FileStorage.FileInfo GetPrintForm(long emailId, string filename)
        {
            var fileManager = this.Container.Resolve<IFileManager>();
            try
            {
                var gkhBaseReportDomain = this.Container.ResolveAll<IGkhBaseReport>();
                var report = gkhBaseReportDomain.FirstOrDefault(x => x.Id == "EmailGjiAccepted");
                var userParam = new UserParamsValues();
                userParam.AddValue("Id", emailId);
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
                var file = fileManager.SaveFile(stream, $"{filename}.pdf");
                return file;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                this.Container.Release(fileManager);
               
            }
        }

        private B4.Modules.FileStorage.FileInfo GetPOSPrintForm(long emailId, string filename)
        {
            var fileManager = this.Container.Resolve<IFileManager>();
            try
            {
                var gkhBaseReportDomain = this.Container.ResolveAll<IGkhBaseReport>();
                var report = gkhBaseReportDomain.FirstOrDefault(x => x.Id == "EmailGjiPOSAccepted");
                var userParam = new UserParamsValues();
                userParam.AddValue("Id", emailId);
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
                var file = fileManager.SaveFile(stream, $"{filename}.pdf");
                return file;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                this.Container.Release(fileManager);

            }
        }

    }
}