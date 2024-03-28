namespace Bars.GkhGji.Regions.Voronezh.StateChanges
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using B4.DataAccess;
    using B4.Modules.States;
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Bars.GkhGji.Regions.Voronezh.ASDOU;
    using Castle.Windsor;
    using Entities;

    public class ASDOUSendRule : IRuleChangeStatus
    {
        /// <summary>
        /// Домен сервис <see cref="LogOperation"/>
        /// </summary>
        public IDomainService<LogOperation> LogOperationDomainService { get; set; }

        public IDomainService<AppealCitsAnswer> AppealCitsAnswerDomainService { get; set; }
        public IDomainService<AppealCitsStatSubject> AppealCitsStatSubjectDomainService { get; set; }
        public IDomainService<AppealCitsExecutionType> AppealCitsExecutionTypeDomainService { get; set; }

        public virtual IWindsorContainer Container { get; set; }

        public IFileManager FileManager { get; set; }

        public string Id
        {
            get { return "ASDOUSendRule"; }
        }

        public string Name { get { return "Отправка отчета об исполнении в АС ДОУ"; } }
        public string TypeId { get { return "gji_appeal_citizens"; } }
        public string Description
        {
            get
            {
                return "При переводе статуса будет отправлен отчет об исполнении обращения";
            }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            string log = string.Empty;
            var appealCitsRepo = Container.Resolve<IRepository<AppealCits>>();
            var AppealCitsResolutionDomain = Container.ResolveDomain<AppealCitsResolution>();
            var AppealCitsResolutionExecutorDomain = Container.ResolveDomain<AppealCitsResolutionExecutor>();
            List<SendResult> resultList = new List<SendResult>();
            DateTime ds = DateTime.Now;
            try
            {
               
                RequestExecutionService client = new RequestExecutionService();
                NetworkCredential netCredential = new NetworkCredential("2 ", "2");
            //    Uri uri = new Uri(client.Url);
             //   ICredentials credentials = netCredential.GetCredential(uri, "Basic");
              //  client.Credentials = credentials;
             //   client.PreAuthenticate = true;
                
           //     client.Timeout = 20000;
               
                var appeal = statefulEntity as AppealCits;
                if (appeal.ObjectVersion > 111)
                {
                    return ValidateResult.Yes();
                }
                var answer = AppealCitsAnswerDomainService.GetAll()
                    .Where(x => x.AppealCits.Id == appeal.Id)
                    .Where(x => x.Addressee != null).OrderByDescending(x => x.Id).FirstOrDefault();
                if (answer == null)
                {
                    return ValidateResult.No("Не найдено ответа на обращение с заполненным адресатом");
                }
                string asDouAnswerNum = string.Empty;
                if (!string.IsNullOrEmpty(answer.SerialNumber))
                {
                    asDouAnswerNum = $"69-11/{answer.SerialNumber}-{answer.DocumentNumber}";
                }
                else
                {
                    asDouAnswerNum = answer.DocumentNumber;
                }
                if (string.IsNullOrEmpty(appeal.CaseNumber))
                {
                    return ValidateResult.Yes();
                }
                appeal.ObjectVersion = 111;
                appealCitsRepo.Update(appeal);

                var resIds = AppealCitsResolutionExecutorDomain.GetAll()
                    .Where(z => z.AppealCitsResolution.AppealCits.Id == appeal.Id && z.Surname == "Гончарова")
                    .Select(z=> z.AppealCitsResolution.Id).Distinct().ToList();
                var resolutionList = AppealCitsResolutionDomain.GetAll()
                    .Where(x=> resIds.Contains(x.Id))
                    .Where(x => x.AppealCits.Id == appeal.Id).ToList();
                if (resolutionList == null || resolutionList.Count == 0)
                {
                    return ValidateResult.Yes();
                }
                else if (resolutionList.Count > 0)
                {
                    
                    resolutionList.ForEach(x =>
                    {
                        var employee = AppealCitsResolutionExecutorDomain.GetAll().FirstOrDefault(z => z.AppealCitsResolution == x && z.Surname == "Гончарова");
                        string resContent = x.ResolutionContent != null ? x.ResolutionContent : "";
                        if (employee != null)
                        {
                            //var testres = new FinalReport
                            //{
                            //    infos = GetInfos(answer.AppealCits.Id),
                            //    attachments = GetAttachments(answer),
                            //    author = new Employee
                            //    {
                            //        firstName = employee.Name,
                            //        lastName = employee.Surname,
                            //        middleName = employee.Patronymic
                            //    },
                            //    date = DateTime.Now,
                            //    importId = $"{answer.Id}_{x.Id}_И",
                            //    parentId = x.ImportId,
                            //    text = "Отработано",
                            //    clCodeList = GetStatsubj(appeal.Id),
                            //    reviewResult = new ReviewResult
                            //    {
                            //        type = GetType(appeal.QuestionStatus),
                            //        measuresTaken = true,
                            //        replyDate = answer.DocumentDate.HasValue ? answer.DocumentDate.Value : answer.ObjectCreateDate,
                            //        resultDocument = new ResultDocument
                            //        {
                            //            attachments = GetAttachments(answer),
                            //            orgName = "Гжи воронежской области",
                            //            orgSSTUId = "6bc82a49-6a97-43f4-9e42-a07d417b60b6",
                            //            regDate = answer.DocumentDate.HasValue ? answer.DocumentDate.Value : answer.ObjectCreateDate,
                            //            regNumber = answer.DocumentNumber
                            //        }
                            //    }

                            //};
                            //var testxml = GetRequestElement(testres);


                            if (resContent.Contains("Отклонен итоговый отчет"))
                            {
                                ReviewResult res = null;
                                if (appeal.QuestionStatus == QuestionStatus.Plizdu)
                                {
                                    res = new ReviewResult
                                    {
                                        type = GetType(appeal.QuestionStatus),
                                        measuresTaken = true,
                                        measuresTakenSpecified = true,
                                        replyDateSpecified = true,
                                        replyDate = answer.DocumentDate.HasValue ? answer.DocumentDate.Value : answer.ObjectCreateDate,
                                        resultDocument = new ResultDocument
                                        {
                                            attachments = GetAttachments(answer),
                                            orgName = "Гжи воронежской области",
                                            orgSSTUId = "6bc82a49-6a97-43f4-9e42-a07d417b60b6",
                                            regDate = answer.DocumentDate.HasValue ? answer.DocumentDate.Value : answer.ObjectCreateDate,
                                            regDateSpecified = true,
                                            regNumber = asDouAnswerNum
                                        }
                                    };
                                }
                                else
                                {
                                    res = new ReviewResult
                                    {
                                        type = GetType(appeal.QuestionStatus),
                                        measuresTaken = false,
                                        measuresTakenSpecified = true,
                                        replyDateSpecified = true,
                                        replyDate = answer.DocumentDate.HasValue ? answer.DocumentDate.Value : answer.ObjectCreateDate,
                                        resultDocument = new ResultDocument
                                        {
                                            attachments = GetAttachments(answer),
                                            orgName = "Гжи воронежской области",
                                            orgSSTUId = "6bc82a49-6a97-43f4-9e42-a07d417b60b6",
                                            regDate = answer.DocumentDate.HasValue ? answer.DocumentDate.Value : answer.ObjectCreateDate,
                                            regDateSpecified = true,
                                            regNumber = asDouAnswerNum
                                        }
                                    };
                                }
                                var finalresponce = client.SendFinalReport(new FinalReport
                                {
                                    infos = GetInfos(answer.AppealCits.Id),
                                    attachments = GetAttachments(answer),
                                    author = new Employee
                                    {
                                        firstName = employee.Name,
                                        lastName = employee.Surname,
                                        middleName = employee.Patronymic
                                    },
                                    date = DateTime.Now,
                                    importId = $"{answer.Id}_{x.Id}_И",
                                    parentId = x.ImportId,
                                    text = "Отработано",
                                    clCodeList = GetStatsubj(appeal.Id),
                                    reviewResult = res

                                });
                                resultList.Add(finalresponce);
                                log += $"{finalresponce.code} {finalresponce.message}";
                            }
                            else if (resContent.Contains("Отклонен отчет по резолюции") || employee.IsResponsible == Gkh.Enums.YesNo.No)
                            {
                                var responce = client.SendReport(new Report
                                {
                                    attachments = GetAttachments(answer),
                                    author = new Employee
                                    {
                                        firstName = employee.Name,
                                        lastName = employee.Surname,
                                        middleName = employee.Patronymic
                                    },
                                    date = DateTime.Now,
                                    importId = $"{answer.Id}_{x.Id}_О",
                                    parentId = x.ImportId,
                                    text = "Отработано"
                                });
                                resultList.Add(responce);
                                log += $"{responce.code} {responce.message}";                               
                            }
                            else if(employee.IsResponsible == Gkh.Enums.YesNo.Yes)
                            {
                                var responce = client.SendReport(new Report
                                {
                                    attachments = GetAttachments(answer),
                                    author = new Employee
                                    {
                                        firstName = employee.Name,
                                        lastName = employee.Surname,
                                        middleName = employee.Patronymic
                                    },
                                    date = DateTime.Now,
                                    importId = $"{answer.Id}_{x.Id}_О",
                                    parentId = x.ImportId,
                                    text = "Отработано"
                                });
                                resultList.Add(responce);
                                log += $"{responce.code} {responce.message}";

                                ReviewResult res = null;

                                if (appeal.QuestionStatus == QuestionStatus.Plizdu)
                                {
                                    res = new ReviewResult
                                    {
                                        type = GetType(appeal.QuestionStatus),
                                        measuresTaken = true,
                                        measuresTakenSpecified = true,
                                        replyDateSpecified = true,
                                        replyDate = answer.DocumentDate.HasValue ? answer.DocumentDate.Value : answer.ObjectCreateDate,
                                        resultDocument = new ResultDocument
                                        {
                                            attachments = GetAttachments(answer),
                                            orgName = "Гжи воронежской области",
                                            orgSSTUId = "6bc82a49-6a97-43f4-9e42-a07d417b60b6",
                                            regDate = answer.DocumentDate.HasValue ? answer.DocumentDate.Value : answer.ObjectCreateDate,
                                            regDateSpecified = true,
                                            regNumber = asDouAnswerNum
                                        }
                                    };
                                }
                                else
                                {
                                    res = new ReviewResult
                                    {
                                        type = GetType(appeal.QuestionStatus),
                                        measuresTaken = false,
                                        measuresTakenSpecified = true,
                                        replyDateSpecified = true,
                                        replyDate = answer.DocumentDate.HasValue ? answer.DocumentDate.Value : answer.ObjectCreateDate,
                                        resultDocument = new ResultDocument
                                        {
                                            attachments = GetAttachments(answer),
                                            orgName = "Гжи воронежской области",
                                            orgSSTUId = "6bc82a49-6a97-43f4-9e42-a07d417b60b6",
                                            regDate = answer.DocumentDate.HasValue ? answer.DocumentDate.Value : answer.ObjectCreateDate,
                                            regDateSpecified = true,
                                            regNumber = asDouAnswerNum
                                        }
                                    };
                                }

                                var finalresponce = client.SendFinalReport(new FinalReport
                                {
                                    infos = GetInfos(answer.AppealCits.Id),
                                    attachments = GetAttachments(answer),
                                    author = new Employee
                                    {
                                        firstName = employee.Name,
                                        lastName = employee.Surname,
                                        middleName = employee.Patronymic
                                    },
                                    date = DateTime.Now,
                                    importId = $"{answer.Id}_{x.Id}_И",
                                    parentId = x.ImportId,
                                    text = "Отработано",
                                    clCodeList = GetStatsubj(appeal.Id),
                                    reviewResult = res

                                });
                                resultList.Add(finalresponce);
                                log += $"{finalresponce.code} {finalresponce.message}";
                            }
                        }
                    });
             
                }
                this.LogOperationDomainService.Save(new LogOperation
                {
                    OperationType = Gkh.Enums.LogOperationType.AppealReg,
                    StartDate = ds,
                    EndDate = DateTime.Now,
                    Comment = log
                });

            }
            catch(Exception e)
            {
                this.LogOperationDomainService.Save(new LogOperation
                {
                    OperationType = Gkh.Enums.LogOperationType.AppealReg,
                    StartDate = ds,
                    EndDate = DateTime.Now,
                    Comment = e.Message
                });
                return ValidateResult.No("Не удалось отправить отчет в АС ДОУ. " +e.Message);
            }
            finally
            {
                Container.Release(AppealCitsResolutionDomain);
                Container.Release(appealCitsRepo);
            }
                                      
            return ValidateResult.Yes();
        }

        private XElement GetRequestElement(FinalReport order)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(FinalReport));
                    xmlSerializer.Serialize(streamWriter, order);
                    return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
        }

        private ReviewResultType GetType(QuestionStatus state)
        {
            switch (state)
            {
                case QuestionStatus.Answered:
                    return ReviewResultType.ANSWERED;
                case QuestionStatus.ConsiderationExtended:
                    return ReviewResultType.ANSWERED;
                case QuestionStatus.Explained:
                    return ReviewResultType.EXPLAINED;
                case QuestionStatus.InWork:
                    return ReviewResultType.ANSWERED;
                case QuestionStatus.LeftWithoutAnswer:
                    return ReviewResultType.LEFTWITHOUTANSWER;
                case QuestionStatus.NotSet:
                    return ReviewResultType.ANSWERED;
                case QuestionStatus.NotSupported:
                    return ReviewResultType.NOTSUPPORTED;
                case QuestionStatus.Plizdu:
                    return ReviewResultType.SUPPORTED;
                case QuestionStatus.Supported:
                    return ReviewResultType.SUPPORTED;
                case QuestionStatus.Transferred:
                    return ReviewResultType.TRANSFERRED;

                default: return ReviewResultType.ANSWERED;
            }
        }

        private Attachment[] GetAttachments(AppealCitsAnswer data)
        {
            var fileinfo = FileManager.GetBase64String(data.File);
           
            List<Attachment> att = new List<Attachment>();
            att.Add(new Attachment
            {
                content = Convert.FromBase64String(fileinfo),
                name = $"{data.File.Name}.{data.File.Extention}"
            });
            return att.ToArray();

        }

        private string[] GetStatsubj(long appId)
        {         
           
            List<string> att = new List<string>();
            AppealCitsStatSubjectDomainService.GetAll()
                .Where(x => x.AppealCits.Id == appId)
                .ToList().ForEach(x =>
                {
                    if (x.Subsubject != null)
                    {
                        att.Add($"{x.Subject.SSTUCode}.{x.Subsubject.Code}");
                    }
                    else
                    {
                        att.Add(x.Subject.SSTUCode);
                    }
                });
                
            return att.ToArray();

        }

        private string[] GetInfos(long appId)
        {

            List<string> att = new List<string>();
            AppealCitsExecutionTypeDomainService.GetAll()
                .Where(x => x.AppealCits.Id == appId)
                .ToList().ForEach(x =>
                {
                    if (x.AppealExecutionType != null)
                    {
                        att.Add(x.AppealExecutionType.Name);
                    }
                   
                });

            return att.ToArray();

        }

    }
}
