using Bars.B4;
using Bars.B4.Application;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Regions.Habarovsk.Services.DataContracts.SyncAppealCitFromEDM;
using Bars.GkhGji.Regions.Habarovsk.Services.ServiceContracts;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
using Castle.Core.Internal;
using Castle.Windsor;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;
// TODO: Заменить Itextsharp
//using iTextSharp.text.pdf.qrcode;
using Bars.B4.Utils;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;

namespace Bars.GkhGji.Regions.Habarovsk.Services.Impl
{
    public class AppealCitFromEDMService : IAppealCitFromEDMService
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<AppealCits> AppealCitsDomain { get; set; }
        public IDomainService<SocialStatus> SocialStatusDomain { get; set; }
        public IDomainService<KindStatementGji> KindStatementDomain { get; set; }
        public IDomainService<RelatedAppealCits> RelatedCitsDomain { get; set; }
        public IDomainService<StatSubjectGji> StatSubjectDomain { get; set; }
        public IDomainService<AppealCitsStatSubject> AppealCitsStatSubjectDomain { get; set; }
        public IDomainService<AppealCitsAttachment> AttachmentDomain { get; set; }
        public IDomainService<AppealCitsSource> AppealCitsSourceDomain { get; set; }
        public IDomainService<RevenueSourceGji> RevenueSourceGjiDomain { get; set; }
        public IDomainService<RevenueFormGji> RevenueFormGjiDomain { get; set; }
        public IFileManager FileManager { get; set; }

        public AppealCitFromEDMService()
        {
            this.Container = ApplicationContext.Current.Container;
        }

        public string GetAppealCitFromEDM(RegCardToBarsDto request)
        {
            AppealCitsDomain = Container.ResolveDomain<AppealCits>();
            SocialStatusDomain = Container.ResolveDomain<SocialStatus>();
            KindStatementDomain = Container.ResolveDomain<KindStatementGji>();
            RelatedCitsDomain = Container.ResolveDomain<RelatedAppealCits>();
            StatSubjectDomain = Container.ResolveDomain<StatSubjectGji>();
            AppealCitsStatSubjectDomain = Container.ResolveDomain<AppealCitsStatSubject>();
            AppealCitsSourceDomain = Container.ResolveDomain<AppealCitsSource>();
            AttachmentDomain = Container.ResolveDomain<AppealCitsAttachment>();
            RevenueSourceGjiDomain = Container.ResolveDomain<RevenueSourceGji>();
            RevenueFormGjiDomain = Container.ResolveDomain<RevenueFormGji>();

            var oldAppeal = AppealCitsSourceDomain.GetAll()
                .Where(x => x.RevenueSource.Code == "24")
                .Where(x => x.AppealCits.ArchiveNumber.Equals(request.ComplaintId.ToString()))
                .Select(x => x.AppealCits)
                .FirstOrDefault();

            if (oldAppeal == null)
            {
                try
                {
                    var parseSeccess = Enum.TryParse(request.BarsComplaintSourceName, out TypeCorrespondent typeCorrespondent);

                    var appealFromEDM = new AppealCits()
                    {
                        Correspondent = string.Join("; ", request.Applicants.Select(x => x.Name)),
                        CorrespondentAddress = string.Join("; ", request.Applicants.Select(x => x.Address)),
                        Email = string.Join("; ", request.Applicants.Select(x => x.Email)),
                        Phone = string.Join("; ", request.Applicants.Select(x => x.Phone)),
                        SocialStatus = SocialStatusDomain.GetAll().Where(x => x.Name.Equals(request.Applicants.First().SocialStatuses.First())).FirstOrDefault(),
                        ArchiveNumber = request.ComplaintId.ToString(),
                        TypeCorrespondent = parseSeccess ? typeCorrespondent : TypeCorrespondent.CitizenHe,
                        DateFrom = request.DeliveryDate,
                        Year = request.DeliveryYear,
                        CheckTime = request.ControlDueDate,
                        KindStatement = KindStatementDomain.GetAll().Where(x => x.Name.Equals(request.DeliveryMethod)).FirstOrDefault(),
                        QuestionsCount = request.QuestionCount,
                        Description = request.Content,
                        File = GetAttachmentFromEDM(request.ComplaintFile.File),
                        Municipality = request.SubjectName,
                        RealityAddresses = request.AdmLocationSubjectName,
                        StatementSubjects = string.Join("; ", request.ComplaintQuestionThemeNames)
                    };

                    if (request.MainCaseFile != null && request.MainCaseFile.Id != null && request.MainCaseFile.Id.ToString() != "")
                    {
                        var prevAppeal = AppealCitsSourceDomain.GetAll()
                            .Where(x => x.RevenueSource.Code == "24")
                            .Where(x => x.AppealCits.ArchiveNumber.Equals(request.MainCaseFile.Id.ToString()))
                            .Select(x => x.AppealCits)
                            .FirstOrDefault();

                        appealFromEDM.PreviousAppealCits = prevAppeal;
                    }

                    AppealCitsDomain.Save(appealFromEDM);

                    var appealSource = new AppealCitsSource
                    {
                        AppealCits = appealFromEDM,
                        RevenueDate = DateTime.Now,
                        RevenueSource = RevenueSourceGjiDomain.GetAll().Where(x => x.Code == "24").FirstOrDefault(),
                        RevenueSourceNumber = request.ComplaintId.ToString(),
                        RevenueForm = RevenueFormGjiDomain.GetAll().Where(x => x.Code == "13").FirstOrDefault(),
                    };
                    AppealCitsSourceDomain.Save(appealSource);

                    if (request.CardToCardLinks.Count > 0)
                    {
                        var relaitedAppeals = AppealCitsSourceDomain.GetAll()
                            .Where(x => x.RevenueSource.Code == "24")
                            .Where(x => request.CardToCardLinks.Select(y => y.Id.ToString()).Contains(x.AppealCits.ArchiveNumber))
                            .Select(x => x.AppealCits);

                        foreach (var relaitedappeal in relaitedAppeals)
                        {
                            var relate = new RelatedAppealCits
                            {
                                Parent = relaitedappeal,
                                Children = appealFromEDM
                            };

                            RelatedCitsDomain.Save(relate);
                        }
                    }

                    if (request.ComplaintQuestionThemeNames.Count > 0)
                    {
                        var statSubject = StatSubjectDomain.GetAll()
                            .Where(x => request.ComplaintQuestionThemeNames.Contains(x.Name))
                            .FirstOrDefault();

                        if (statSubject != null)
                        {
                            var appealCitsStatSubject = new AppealCitsStatSubject
                            {
                                AppealCits = appealFromEDM,
                                Subject = statSubject
                            };

                            AppealCitsStatSubjectDomain.Save(appealCitsStatSubject);
                        }
                    }

                    if (request.Attachments.Count > 0)
                    {
                        foreach (var attachment in request.Attachments)
                        {
                            var appealCitsAttach = new AppealCitsAttachment
                            {
                                AppealCits = appealFromEDM,
                                FileInfo = GetAttachmentFromEDM(attachment.File),
                                Name = attachment.File.Name
                            };

                            AttachmentDomain.Save(appealCitsAttach);
                        }
                    }

                    return JsonConvert.SerializeObject(200);
                }
                catch (DataAccessException e)
                {
                    var error = new ErrorDto
                    {
                        Status = 400,
                        Message = e.Message
                    };

                    return JsonConvert.SerializeObject(error);
                }
                catch
                {
                    var error = new ErrorDto
                    {
                        Status = 400,
                        Message = "Некорректная информация по обращению"
                    };

                    return JsonConvert.SerializeObject(error);
                }
            }
            else
            {
                var error = new ErrorDto
                {
                    Status = 400,
                    Message = "Данное обращение уже зарегестрировано"
                };

                return JsonConvert.SerializeObject(error);
            }
        }

        private FileInfo GetAttachmentFromEDM(ObjectMinDto file)
        {
            FileManager = Container.Resolve<IFileManager>();

            string url = $"http://172.18.205.54:4346/containers/{file.Id}";

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //request.Credentials = CredentialCache.DefaultCredentials;
                request.Method = "GET";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();

                var attachment = FileManager.SaveFile(dataStream, file.Name);

                return attachment;
            }
            catch (Exception e)
            {
                throw new DataAccessException($"Не удалось получеть файл из контейнера {file.Id}", e);
            }
        }
    }
}
