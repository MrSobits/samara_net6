namespace Bars.GkhGji.Regions.Habarovsk.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Habarovsk.Services.ServiceContracts;
    using Castle.Windsor;
    using Remotion.Linq.Utilities;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Repositories;
    using Bars.GkhGji.Regions.Habarovsk.Services.DataContracts.SyncAppealCit;
    using Bars.GkhGji.Regions.Habarovsk.Services.DataContracts;
    using System.Security.Cryptography;
    using System.Text;
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Ionic.Zip;
    using Ionic.Zlib;
    using Bars.B4.Modules.Security;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Regions.Habarovsk.Entities;
    using System.IO;
    using File = DataContracts.SyncAppealCit.File;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.GkhGji.Regions.Habarovsk.Enums;

    /// <summary>
    /// Сервис сведений об обращениях граждан
    /// </summary>
    public partial class AppealCitService : IAppealCitService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }
        public IFileManager FileManager { get; set; }

        public IRepository<AppealCits> AppealCitsRepo { get; set; }
        public IRepository<State> StateDomain { get; set; }
        public IDomainService<StatSubjectGji> StatSubjectGjiDomain { get; set; }
        public IDomainService<StatSubjectSubsubjectGji> StatSubjectSubsubjectGjiDomain { get; set; }
        public IDomainService<StatSubsubjectGji> StatSubsubjectGjiGjiDomain { get; set; }
        public IDomainService<AppealCitsStatSubject> AppealCitsStatSubjectDomain { get; set; }
        public IDomainService<AppealCits> AppealCitsDomain { get; set; }
        public IDomainService<AppealCitsAnswer> AppealCitsAnswerDomain { get; set; }
        public IDomainService<AppealCitsRealityObject> AppealCitsRealityObjectDomain { get; set; }
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }
        public IDomainService<RelatedAppealCits> RelatedAppealCitsDomain { get; set; }
        public IDomainService<AppealCitsAttachment> AppealCitsAttachmentDomain { get; set; }
        public IDomainService<KindStatementGji> KindStatementGjiDomain { get; set; }
        public IDomainService<AppealCitsSource> AppealCitsSourceDomain { get; set; }
        public IDomainService<RevenueSourceGji> RevenueSourceGjiDomain { get; set; }
        public IDomainService<RevenueFormGji> RevenueFormGjiDomain { get; set; }
        public IDomainService<StateHistory> StateHistoryDomain { get; set; }
        public IDomainService<ProtocolOSPRequest> ProtocolOSPRequestDomain { get; set; }
        public IDomainService<AppealCitsResolution> AppealCitsResolutionDomain { get; set; }
        public IDomainService<OwnerProtocolTypeDecision> ProtocolTypeDecisionDomain { get; set; }

        public IDomainService<AppealCitsResolutionExecutor> AppealCitsResolutionExecutorDomain { get; set; }

        /// <summary>
        /// Импорт сведений об обращении граждан
        /// </summary>
        /// <returns></returns>
        public AppealCitResult GetAppealCit(AppealCitRecord appealCitRecord, string token)
        {
            if (!ValidateToken(token))
            {
                return new AppealCitResult
                {
                    Code = 1,
                    Message = "Некорректный токен"
                };
            }

            AppealCits appealCit;
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                var existingAppeal = AppealCitsDomain.GetAll().FirstOrDefault(x => x.CaseNumber == appealCitRecord.Id);
                if (existingAppeal != null)
                {
                    return new AppealCitResult
                    {
                        Code = 10,
                        Message = $"Обращение с ID {appealCitRecord.Id} уже присутствует в системе"
                    };
                }
                else
                {
                    existingAppeal = AppealCitsSourceDomain.GetAll().Where(x => x.RevenueSourceNumber == appealCitRecord.AppealRegistrationNumber && x.RevenueDate == appealCitRecord.AppealRegistrationDate.ToDateTime()).Select(x=> x.AppealCits).FirstOrDefault();
                    if (existingAppeal != null)
                    {
                        return new AppealCitResult
                        {
                            Code = 10,
                            Message = $"Обращение №{appealCitRecord.AppealRegistrationNumber} от {appealCitRecord.AppealRegistrationDate} уже присутствует в системе"
                        };
                    }
                }
                try
                {
                    appealCit = new AppealCits
                    {
                        CaseNumber = appealCitRecord.Id,
                   //     DocumentNumber = appealCitRecord.AppealRegistrationNumber,
                        DateFrom = appealCitRecord.AppealRegistrationDate.ToDateTime(),
                        Correspondent = !string.IsNullOrEmpty(appealCitRecord.Correspondent)? appealCitRecord.Correspondent:"Коллективное обращение",
                        TypeCorrespondent = TypeCorrespondent.CitizenHe,
                        CorrespondentAddress = appealCitRecord.CorrespondentAddress,
                        CheckTime = appealCitRecord.AppealTerm.ToDateTime(),
                        Email = appealCitRecord.Email,
                        KindStatement = KindStatementGjiDomain.GetAll().FirstOrDefault(x => x.Code == "06")
                    };
                    AppealCitsDomain.Save(appealCit);
                    var source = RevenueSourceGjiDomain.GetAll().FirstOrDefault(x => x.Code == "03");
                    if (source != null)
                    {
                        AppealCitsSourceDomain.Save(new AppealCitsSource
                        {
                            AppealCits = appealCit,
                            RevenueDate = appealCitRecord.AppealRegistrationDate.ToDateTime(),
                            RevenueSource = source,
                            RevenueSourceNumber = appealCitRecord.AppealRegistrationNumber,
                            RevenueForm = RevenueFormGjiDomain.GetAll().FirstOrDefault(x => x.Code == "06")

                        });
                    }

                    GetSubject(appealCitRecord.AppealSubjects, appealCit);
                    if (appealCitRecord.Files != null)
                    {
                        foreach (var file in appealCitRecord.Files.Where(x => !string.IsNullOrEmpty(x.Base64)))
                        {
                            var fromBase64 = Convert.FromBase64String(file.Base64);
                            var fileExt = GetNameAndExtention(file.FileName);
                            var appealFile = new AppealCitsAttachment
                            {
                                AppealCits = appealCit,
                                Name = fileExt[0],
                                Description = $"{file.TypeDocument} N{file.NumberDocument} от {file.DocumentDate}",
                                FileInfo = FileManager.SaveFile(new FileData(fileExt[0], fileExt[1], fromBase64)),
                            };
                            AppealCitsAttachmentDomain.Save(appealFile);
                        }
                    }
                    //формируем архив
                    appealCit.File = GetArchive(appealCit.Id, appealCitRecord.AppealRegistrationNumber);
                    if (appealCit != null)
                    {
                        AppealCitsDomain.Update(appealCit);
                    }
                    if (appealCitRecord.Resolutions != null)
                    {
                        foreach (var res in appealCitRecord.Resolutions.Where(x => !string.IsNullOrEmpty(x.ResolutionText)))
                        {
                            var appealRes = new AppealCitsResolution
                            {
                                AppealCits = appealCit,
                                ResolutionText = res.ResolutionText,
                                ResolutionAuthor = res.ResolutionAuthor,
                                ResolutionDate = res.ResolutionDate.ToDateTime(),
                                ResolutionContent = res.ResolutionContent,
                                ResolutionTerm = res.ResolutionTerm.ToDateTime(),
                                ImportId = res.ImportId,
                                ParentId = res.ParentId,
                                Executed = Gkh.Enums.YesNoNotSet.NotSet //оно не приходит
                            };
                            AppealCitsResolutionDomain.Save(appealRes);

                            if (res.ResolutionsExecutors != null)
                            {
                                foreach (var resEx in res.ResolutionsExecutors.Where(x => !string.IsNullOrEmpty(x.Name)))
                                {
                                    var appealResEx = new AppealCitsResolutionExecutor
                                    {
                                        AppealCitsResolution = appealRes,
                                        Name = resEx.Name,
                                        Surname = resEx.Surname,
                                        Patronymic = resEx.Patronymic,
                                        Comment = resEx.Comment,
                                        PersonalTerm = resEx.PersonalTerm.ToDateTime(),
                                        IsResponsible = resEx.IsResponsible ? Gkh.Enums.YesNo.Yes : Gkh.Enums.YesNo.No
                                    };
                                    AppealCitsResolutionExecutorDomain.Save(appealResEx);
                                }
                            }
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new AppealCitResult
                    {
                        Code = 2,
                        Message = "Не удалось сохранить запись. Ошибка: " + e
                    };
                }
            }

            string resultAppCitRo = SaveAppCitRo(appealCit);
            string resultAppealRelated = SaveAppealRelated(appealCit);

            AppealCitResult result = new AppealCitResult
            {
                Code = 0,
                Message = "Успешно",
                AppealID = appealCit.Id.ToString()
            };

            return result;
        }

        /// <summary>
        /// Импорт сведений об обращении граждан
        /// </summary>
        /// <returns></returns>
        public AppealCitResult SendOSSPApproveRequest(OSSPApproveRequest request)
        {
            RealityObject ro = null;
            try
            {
                long roid = Convert.ToInt64(request.HouseId);
                ro = RealityObjectDomain.GetAll().FirstOrDefault(x => x.Id == roid);
            }
            catch
            {
                
            }
            if (ro == null)
            {
                ro = RealityObjectDomain.GetAll().FirstOrDefault(x => x.HouseGuid == request.FiasGuid);
            }

            long? ownProtTypeId = null;
            try
            {
                ownProtTypeId = Convert.ToInt64(request.OwnerProtocolType);
            }
            catch
            {
                
            }


            ProtocolOSPRequest newReq = new ProtocolOSPRequest
            {
                RoFiasGuid = request.FiasGuid,
                Address = request.Address,
                Municipality = ro != null ? $"{ro.Municipality.Name}" : "",
                Approved = FuckingOSSState.Work,
                RequestNumber = request.RequestNumber,
                Note = request.Note,
                DocDate = GetSafeDateFromString(request.DocDate),
                DocNumber = request.DocNumber,
                FIO = request.Fio,
                Room = request.Room,
                RealityObject = ro,
                UserEsiaGuid = request.EsiaId,
                Email = request.Email,
                Date = DateTime.Now,
                ApplicantType = !string.IsNullOrEmpty(request.AttorneyFio)? OSSApplicantType.Agent:OSSApplicantType.Owner,
                PhoneNumber = request.PhoneNumber,
                ProtocolNum = request.ProtocolNum,
                ProtocolDate = GetSafeDateFromString(request.ProtocolDate),
                DateFrom = GetSafeNotNullDateFromString(request.DateFrom),
                DateTo = GetSafeNotNullDateFromString(request.DateTo),
                AttorneyDate = GetSafeDateFromString(request.AttorneyDate),
                AttorneyFio = request.AttorneyFio,
                AttorneyNumber = request.AttorneyNumber,
                CadastralNumber = request.CadastralNumber,
                GjiId = "",
                OwnerProtocolType = ownProtTypeId.HasValue && ownProtTypeId.Value>1? new OwnerProtocolTypeDecision {Id = ownProtTypeId.Value}:null
            };
            if (request.File != null)
            {
                var fromBase64 = Convert.FromBase64String(request.File.Base64);
                var fileExt = GetNameAndExtention(request.File.FileName);
                if (newReq.ApplicantType == OSSApplicantType.Owner)
                {
                    newReq.FileInfo = FileManager.SaveFile(new FileData(fileExt[0], fileExt[1], fromBase64));
                }
                else
                {
                    newReq.AttorneyFile = FileManager.SaveFile(new FileData(fileExt[0], fileExt[1], fromBase64));
                }
            }
            ProtocolOSPRequestDomain.Save(newReq);
            return new AppealCitResult
            {
                Code = 0,
                AppealID = newReq.Id.ToString(),
                Message = "Ваше заявление принято к рассмотрению. Cрок рассмотрения заявления составляет 10 дней"
            };
        }
        public GetProtocolTypesResult GetProtocolTypes()
        {
            var req = ProtocolTypeDecisionDomain.GetAll().Where(x=> x.Id>1).OrderBy(x=> x.Id).ToList();
            if (req.Count > 0)
            {
                List<ProtocolType> stateList = new List<ProtocolType>();
                stateList.Add(new ProtocolType
                {
                    Id = "1",
                    Name = "Не указывать"
                });
                foreach (var r in req)
                {
                    stateList.Add(new ProtocolType
                    {
                       Id = r.Id.ToString(),
                       Name = r.Name
                    });
                }
                return new GetProtocolTypesResult
                {
                    AppealCitResult = new AppealCitResult
                    {
                        Code = 0,
                    },
                    ProtocolTypes = stateList.ToArray()
                };
            }
            return new GetProtocolTypesResult
            {
                AppealCitResult = new AppealCitResult
                {
                    Code = 1,
                    Message = "Список пуст"
                }
            };
        }


        public OSSRequestStateResponce GetOSSRequestsState(string erknmid)
        {
            var req = ProtocolOSPRequestDomain.GetAll().Where(x => x.UserEsiaGuid == erknmid).ToList();
            if (req.Count > 0)
            {
                List<OSSRequestState> stateList = new List<OSSRequestState>();
                foreach (var r in req)
                {
                    stateList.Add(new OSSRequestState
                    {
                        RequestId = r.Id.ToString(),
                        State = GetState(r.Approved, r.State),
                        Comment = !string.IsNullOrEmpty(r.ResolutionContent)? r.ResolutionContent:""
                    });
                }
                return new OSSRequestStateResponce
                {
                    AppealCitResult = new AppealCitResult
                    {
                        Code = 0,
                    },
                    OSSRequestsState = stateList.ToArray()
                };
            }
            return new OSSRequestStateResponce
            {
                AppealCitResult = new AppealCitResult
                {
                    Code = 1,
                    Message = "Заявки не обнаружены"
                }
            };
        }

        public OSSRequestHistoryResponce GetOSSRequestHistory(string requestId)
        {
            var reqId = Convert.ToInt64(requestId);
            if (reqId == 0)
            {
                return new OSSRequestHistoryResponce
                {
                    AppealCitResult = new AppealCitResult
                    {
                        Code = 404,
                        Message = "Запрос не найден в АИС ГЖИ"
                    }
                };
            }
            var req = ProtocolOSPRequestDomain.Get(reqId);//HH:mm:SS
            List<OSSRequestHistory> hist = new List<OSSRequestHistory>();
            hist.Add(new OSSRequestHistory
            {
                NoticeDate = req.ObjectCreateDate.ToString("dd.MM.yyyy"),
                NoticeTime = req.ObjectCreateDate.ToString("HH:mm:ss"),
                RequestId = requestId,
                NoticeText = "Ваше заявление принято к рассмотрению. Cрок рассмотрения заявления составляет 10 дней"
            });
            var statetransfer = StateHistoryDomain.GetAll().Where(x => x.TypeId == "oss_request" && x.EntityId == reqId).OrderBy(x => x.ChangeDate).ToList();
            foreach (var t in statetransfer)
            {
                if (!string.IsNullOrEmpty(t.FinalState.Description))
                {
                    hist.Add(new OSSRequestHistory
                    {
                        NoticeDate = t.ChangeDate.ToString("dd.MM.yyyy"),
                        NoticeTime = t.ChangeDate.ToString("HH:mm:ss"),
                        RequestId = requestId,
                        NoticeText = t.FinalState.Description
                    });
                }
            }
            if (req.State.FinalState)
            {
                string notice = string.Empty;
                switch (req.Approved)
                {
                    case FuckingOSSState.No:
                        notice = "Мотивированный отказ по причине: " + req.ResolutionContent;
                        break;
                    case FuckingOSSState.NotSet:
                        notice = "Уважаемый заявитель! По заданным параметрам протокол ОСС не найден в архиве";
                        break;                   
                    case FuckingOSSState.Yes:
                        notice = "Предоставлена копия протокола ОСС";
                        break;                   
                }
                if (!string.IsNullOrEmpty(notice))
                {
                    hist.Add(new OSSRequestHistory
                    {
                        NoticeDate = req.ObjectEditDate.ToString("dd.MM.yyyy"),
                        NoticeTime = req.ObjectEditDate.ToString("HH:mm:ss"),
                        RequestId = requestId,
                        NoticeText = notice
                    });
                }
            }
            return new OSSRequestHistoryResponce
            {
                AppealCitResult = new AppealCitResult
                {
                    Code = 0,
                },
                OSSRequestHistory = hist.ToArray()
            };
        }

        /// <summary>
        /// Импорт сведений об обращении граждан
        /// </summary>
        /// <returns></returns>
        public SendOSSPApproveResult GetOSSProotocols(string requestId)
        {
            var reqId = Convert.ToInt64(requestId);
            if (reqId == 0)
            {
                return new SendOSSPApproveResult
                {
                    AppealCitResult = new AppealCitResult
                    {
                        Code = 404,
                        Message = "Запрос не найден в АИС ГЖИ"
                    }
                };
            }
            var req = ProtocolOSPRequestDomain.Get(reqId);
            if (req != null && req.Approved == FuckingOSSState.Yes)
            {
                var protDomain = this.Container.Resolve<IDomainService<PropertyOwnerProtocols>>();
                var protList = new List<OSSProtocol>();
                protList.Add(new OSSProtocol
                {
                    File = new File
                    {
                        Base64 = FileManager.GetBase64String(req.ProtocolFile),
                        DocumentDate = req.ProtocolDate.HasValue ? req.ProtocolDate.Value.ToShortDateString() : req.ObjectCreateDate.ToShortDateString(),
                        FileName = req.ProtocolFile.FullName,
                        NumberDocument = req.ProtocolNum,
                        TypeDocument = "Протокол общего собрания собственников"
                    },
                    ProtocolDate = req.ProtocolDate.HasValue ? req.ProtocolDate.Value.ToShortDateString() : req.ObjectCreateDate.ToShortDateString(),
                    RegDate ="",
                    RegNumber = ""
                });

                return new SendOSSPApproveResult
                {
                    AppealCitResult = new AppealCitResult
                    {
                        Code = 0,
                    },
                    OSSProtocols = protList.ToArray()
                };
            }
            if (req != null && req.Approved == FuckingOSSState.No)
            {
                return new SendOSSPApproveResult
                {
                    AppealCitResult = new AppealCitResult
                    {
                        Code = 4,
                        Message = !string.IsNullOrEmpty(req.ResolutionContent)? req.ResolutionContent: "Отказано в предоставлении копии протокола ОСС"
                    }
                };
            }
            if (req != null && req.Approved == FuckingOSSState.Work)
            {
                return new SendOSSPApproveResult
                {
                    AppealCitResult = new AppealCitResult
                    {
                        Code = 3,
                        Message = "Ваше заявление находится на проверке в ГЖИ Воронежской области"
                    }
                };
            }
            if (req != null && req.Approved == FuckingOSSState.NotSet)
            {
                return new SendOSSPApproveResult
                {
                    AppealCitResult = new AppealCitResult
                    {
                        Code = 404,
                        Message = "Протокол ОСС не найден в архиве"
                    }
                };
            }
            return new SendOSSPApproveResult
            {
               AppealCitResult = new AppealCitResult
               {
                   Code = 1,
                   Message = "Необходимо подать заявку на доступ к протколам ОСС"
               }               
            };
        }

        private DateTime? GetSafeNotNullDateFromString(string dt)
        {
            if (string.IsNullOrEmpty(dt))
            {
                return DateTime.Now;
            }
            try
            {
                var dtime = Convert.ToDateTime(dt);
                return dtime;
            }
            catch
            {
                return DateTime.Now;
            }
            return DateTime.Now;
        }

        private DateTime? GetSafeDateFromString(string dt)
        {
            if (string.IsNullOrEmpty(dt))
            {
                return null;
            }
            try
            {
                var dtime = Convert.ToDateTime(dt);
                return dtime;
            }
            catch
            {
                return null;
            }
            return null;
        }

        /// <summary>
        /// Импорт сведений об обращении граждан через портал
        /// </summary>
        /// <returns></returns>
        public AppealCitPortalResult ImportPortalAppeal(PortalAppeal appealCitRecord, string token)
        {
            if (!ValidateTokenPortal(token))
            {
                return new AppealCitPortalResult
                {
                    Code = 1,
                    Message = "Некорректный токен",
                    State = ""
                };
            }

            AppealCits appealCit;
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                var existingAppeal = AppealCitsDomain.GetAll().FirstOrDefault(x => x.CaseNumber == appealCitRecord.Id);
                if (existingAppeal != null)
                {
                    return new AppealCitPortalResult
                    {
                        Code = 10,
                        Message = $"Обращение с ID {appealCitRecord.Id} уже присутствует в системе",
                        State = existingAppeal.State.Name
                    };
                }
                try
                {
                    var kindStat = KindStatementGjiDomain.GetAll()
                        .Where(x => x.Code == "15").FirstOrDefault();
                    appealCit = new Bars.GkhGji.Entities.AppealCits
                    {
                        CaseNumber = appealCitRecord.Id,
                        Description = appealCitRecord.AppealText,
                        DocumentNumber = appealCitRecord.AppealRegistrationNumber,
                        DateFrom = appealCitRecord.AppealDate.ToDateTime(),
                        Correspondent = appealCitRecord.Correspondent,
                        KindStatement = kindStat != null? kindStat:null,
                        CorrespondentAddress = appealCitRecord.CorrespondentAddress,
                        QuestionStatus = QuestionStatus.InWork,
                        Email = appealCitRecord.Email
                    };
                    AppealCitsDomain.Save(appealCit);

                    var source = RevenueSourceGjiDomain.GetAll().FirstOrDefault(x => x.Code == "512");
                    if (source != null)
                    {
                        AppealCitsSourceDomain.Save(new AppealCitsSource
                        {
                            AppealCits = appealCit,
                            RevenueDate = appealCitRecord.AppealRegistrationDate.ToDateTime(),
                            RevenueSource = source,
                            RevenueSourceNumber = appealCitRecord.Id,
                            RevenueForm = RevenueFormGjiDomain.GetAll().FirstOrDefault(x=> x.Code == "12")

                        });
                    }

                    if (appealCitRecord.Files != null)
                    {
                        foreach (var file in appealCitRecord.Files.Where(x => !string.IsNullOrEmpty(x.Base64)))
                        {
                            var fromBase64 = Convert.FromBase64String(file.Base64);
                            var fileExt = GetNameAndExtention(file.FileName);
                            var appealFile = new AppealCitsAttachment
                            {
                                AppealCits = appealCit,
                                Name = fileExt[0],
                                Description = $"{file.TypeDocument} N{file.NumberDocument} от {file.DocumentDate}",
                                FileInfo = FileManager.SaveFile(new FileData(fileExt[0], fileExt[1], fromBase64)),
                            };
                            AppealCitsAttachmentDomain.Save(appealFile);
                        }
                    }                  

                    transaction.Commit();

                    appealCit.File = GetArchive(appealCit.Id, appealCitRecord.AppealRegistrationNumber);
                    AppealCitsDomain.Update(appealCit);

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new AppealCitPortalResult
                    {
                        Code = 2,
                        Message = "Не удалось сохранить запись. Ошибка: " + e
                    };
                }
            }

            string resultAppCitRo = SaveAppCitRo(appealCit);
            string resultAppealRelated = SaveAppealRelated(appealCit);

            AppealCitPortalResult result = new AppealCitPortalResult
            {
                Code = 0,
                Message = "Обращение зарегистрировано в системе",
                State = "В работе",
                AppealID = appealCit.Number.ToString()
            };

            return result;
        }

        /// <summary>
        /// Импорт сведений об статусе отчета
        /// </summary>
        /// <returns></returns>
        public ReportResult SetReportState(ReportState reportState, string token)
        {
            if (!ValidateToken(token))
            {
                return new ReportResult
                {
                    Code = 1,
                    Message = "Некорректный токен",
                    ReportId = reportState.ReportId
                };
            }
          
            try
            {
                long resId = Convert.ToInt64(reportState.ReportId.Split('_')[1]);

                var report_type = reportState.ReportId.Split('_')[2];

                if (resId> 0)
                {
                    var resolution = AppealCitsResolutionDomain.Get(resId);
                    if (reportState.Accepted)
                    {
                        resolution.Executed = Gkh.Enums.YesNoNotSet.Yes;
                        resolution.ResolutionContent = "";
                        AppealCitsResolutionDomain.Update(resolution);
                    }
                    else
                    {
                        resolution.ResolutionContent = report_type == "И"? "Отклонен итоговый отчет с комментарием: " + reportState.DeclineReason: "Отклонен отчет по резолюции с комментарием: " + reportState.DeclineReason;
                        resolution.Executed = Gkh.Enums.YesNoNotSet.No;
                        AppealCitsResolutionDomain.Update(resolution);
                        State state = StateDomain.GetAll()
                            .Where(x => x.TypeId == "gji_appeal_citizens" && x.Name == "В работе").FirstOrDefault();
                        var appeal = AppealCitsRepo.Get(resolution.AppealCits.Id);
                        if (state != null)
                        {
                            appeal.State = state;
                            AppealCitsRepo.Update(appeal);
                        }

                    }
                }
                return new ReportResult
                {
                    Code = 0,
                    Message = "Информация о статусе отчета принята",
                    ReportId = reportState.ReportId
                };

            }
            catch (Exception e)
            {
               return new ReportResult
                {
                    Code = 2,
                    Message = "Ошибка смены статуса обращения",
                    ReportId = reportState.ReportId
                };
            }
        }

        /// <summary>
        /// Импорт сведений об обращении граждан через портал
        /// </summary>
        /// <returns></returns>
        public AppealCitStateResult GetState(string Id, string token)
        {
            if (!ValidateTokenPortal(token))
            {
                return new AppealCitStateResult
                {
                    Code = 1,
                    Message = "Некорректный токен",
                    State = ""
                };
            }
            var existingAppeal = AppealCitsDomain.GetAll().FirstOrDefault(x => x.CaseNumber == Id);
            if (existingAppeal != null)
            {
                if (!existingAppeal.State.FinalState)
                {
                    return new AppealCitStateResult
                    {
                        State = "В работе",
                        AppealID = existingAppeal.Number,
                        RegDate = existingAppeal.ObjectCreateDate.ToShortDateString(),
                        Message = "Обращение находится в работе ГЖИ"
                    };
                }
                else
                {
                    var answer = AppealCitsAnswerDomain.GetAll()
                        .Where(x => x.AppealCits.Id == existingAppeal.Id)
                        .Where(x => x.Addressee != null).FirstOrDefault();
                    AppealCitStateResult result = new AppealCitStateResult
                    {
                        Code = 0,
                        Message = "Работа с обращением завершена",
                        State = "Закрыто",
                        RegDate = existingAppeal.ObjectCreateDate.ToShortDateString(),
                        AppealID = existingAppeal.Number,
                        File = new File
                        {
                            Base64 = FileManager.GetBase64String(answer.File),
                            DocumentDate = answer.DocumentDate.HasValue? answer.DocumentDate.Value.ToShortDateString():answer.ObjectCreateDate.ToShortDateString(),
                            FileName = $"{answer.File.Name}.{answer.File.Extention}",
                            NumberDocument = answer.DocumentNumber,
                            TypeDocument = answer.DocumentName
                        }
                    };
                    return result;
                }
            }
            else
            {
                return new AppealCitStateResult
                {
                    Code = 1,
                    Message = "Обращение с указанным ИД не найдено в системе",
                    State = ""
                };
            }

     

  
        }

        public string SaveAppCitRo(AppealCits appealCit)
        {
            try
            {
                RealityObject realityObject = RealityObjectDomain.GetAll()
                    .FirstOrDefault(x => x.FiasAddress.AddressName == appealCit.CorrespondentAddress);

                if (realityObject != null)
                {
                    AppealCitsRealityObject appealCitRealObj = new AppealCitsRealityObject
                    {
                        AppealCits = appealCit,
                        RealityObject = realityObject
                    };
                    AppealCitsRealityObjectDomain.Save(appealCitRealObj);
                }
                else
                {
                    return "Дом в ФИАС не найден";
                }
            }
            catch (Exception e)
            {
                return "Не удалось сохранить AppealCitsRealityObject. Ошибка: " + e;
            }

            return "Успешно";
        }

        public string SaveAppealRelated(AppealCits appealCit)
        {
            try
            {
                var appealList = AppealCitsDomain.GetAll()
                    .Where(x => x.Correspondent == appealCit.Correspondent && x.CorrespondentAddress == appealCit.CorrespondentAddress && x.Id != appealCit.Id)
                    .ToList();

                if (appealList.Count == 0)
                {
                    return "Связанных обращений не найдено;";
                }

                appealList.ForEach(x =>
                {
                    RelatedAppealCits relatedAppeal = new RelatedAppealCits
                    {
                        Parent = appealCit,
                        Children = x
                    };
                    RelatedAppealCitsDomain.Save(relatedAppeal);
                });
            }
            catch (Exception e)
            {
                return "Не удалось сохранить RelatedAppealCits. Ошибка: " + e;
            }

            return "Успешно";
        }

        private void GetSubject(AppealSubject[] subj, AppealCits appeal)
        {
            var subDict = StatSubjectGjiDomain.GetAll()
               .Where(x => x.SSTUCode != null && x.SSTUCode != "")
               .ToList()
               .GroupBy(x => x.SSTUCode)
               .ToDictionary(x => x.Key, y => y.First());

            var subsubdict = StatSubjectSubsubjectGjiDomain.GetAll()
                 .Where(x => x.Subsubject.SSTUCodeSub != null && x.Subsubject.SSTUCodeSub != "")
                 .Select(x=> new
                 {
                     Id = x.Id,
                     Code = $"{x.Subject.SSTUCode}.{x.Subsubject.Code}" 
                 })
               .ToList()
               .GroupBy(x => x.Code)
               .ToDictionary(x => x.Key, y => y.First());

            foreach (var s in subj)
            {
                var ind = s.SSTUCode.Split('.').Length;
                if (ind == 5)
                {
                    string subjectName = s.SSTUCode.IndexOf('.') > 0 ? s.SubjectName.Split('.')[0] : s.SubjectName;
                    string subsubjectName = s.SSTUCode.IndexOf('.') > 0 ? s.SubjectName.Split('.')[1] : s.SubjectName;
                    //5 групп знаков кода, присутствует подтематика, ищем пару
                    if (subsubdict.ContainsKey(s.SSTUCode))
                    {
                        var subsubsub = StatSubjectSubsubjectGjiDomain.Get(subsubdict[s.SSTUCode].Id);
                        AppealCitsStatSubject apSSt = new AppealCitsStatSubject
                        {
                            AppealCits = appeal,
                            Subject = subsubsub.Subject,
                            Subsubject = subsubsub.Subsubject

                        };
                        AppealCitsStatSubjectDomain.Save(apSSt);
                    }
                    else
                    {
                        //Ищем/создаем тематику/создаем подтематику
                        string subjectCode = $"{s.SSTUCode.Split('.')[0]}.{s.SSTUCode.Split('.')[1]}.{s.SSTUCode.Split('.')[2]}.{s.SSTUCode.Split('.')[3]}";
                        string subsubcode = s.SSTUCode.Split('.')[4];
                        if (subDict.ContainsKey(subjectCode))
                        {
                            //создаем подтематику
                            StatSubsubjectGji newSubSub = new StatSubsubjectGji
                            {
                                Code = subsubcode,
                                ISSOPR = false,
                                Name = subsubjectName,
                                SSTUCodeSub = subsubcode,
                                SSTUNameSub = subsubjectName
                            };
                            StatSubsubjectGjiGjiDomain.Save(newSubSub);
                            StatSubjectSubsubjectGji stss = new StatSubjectSubsubjectGji
                            {
                                Subject = subDict[subjectCode],
                                Subsubject = newSubSub
                            };
                            StatSubjectSubsubjectGjiDomain.Save(stss);
                            AppealCitsStatSubject apSSt = new AppealCitsStatSubject
                            {
                                AppealCits = appeal,
                                Subject = subDict[subjectCode],
                                Subsubject = newSubSub
                            };
                            AppealCitsStatSubjectDomain.Save(apSSt);
                        }
                        else
                        {
                            //создаем и то и другое
                            StatSubjectGji statSubjectGji = new StatSubjectGji
                            {
                                SSTUCode = subjectCode,
                                Name = subjectName
                            };
                            //создаем подтематику
                            StatSubsubjectGji newSubSub = new StatSubsubjectGji
                            {
                                Code = subsubcode,
                                ISSOPR = false,
                                Name = subsubjectName,
                                SSTUCodeSub = subsubcode,
                                SSTUNameSub = subsubjectName
                            };

                            StatSubjectGjiDomain.Save(statSubjectGji);
                            StatSubsubjectGjiGjiDomain.Save(newSubSub);
                            StatSubjectSubsubjectGji stss = new StatSubjectSubsubjectGji
                            {
                                Subject = statSubjectGji,
                                Subsubject = newSubSub
                            };
                            StatSubjectSubsubjectGjiDomain.Save(stss);
                            AppealCitsStatSubject apSSt = new AppealCitsStatSubject
                            {
                                AppealCits = appeal,
                                Subject = statSubjectGji,
                                Subsubject = newSubSub
                            };
                            AppealCitsStatSubjectDomain.Save(apSSt);
                        }
                    }
                }
                else if (subDict.ContainsKey(s.SSTUCode))
                {
                    AppealCitsStatSubject apSS = new AppealCitsStatSubject
                    {
                        AppealCits = appeal,
                        Subject = subDict[s.SSTUCode]
                    };
                    AppealCitsStatSubjectDomain.Save(apSS);
                }
                else if (!subDict.ContainsKey(s.SSTUCode))
                {
                    StatSubjectGji statSubjectGji = new StatSubjectGji
                    {
                        SSTUCode = s.SSTUCode,
                        Name = s.SubjectName
                    };
                    StatSubjectGjiDomain.Save(statSubjectGji);
                    subDict[statSubjectGji.SSTUCode] = statSubjectGji;


                    AppealCitsStatSubject apSS = new AppealCitsStatSubject
                    {
                        AppealCits = appeal,
                        Subject = subDict[s.SSTUCode]
                    };
                    AppealCitsStatSubjectDomain.Save(apSS);
                }
            }
        }

        private string GetAccessToken()
        {
            var token = $"{DateTime.Now.ToString("dd.MM.yyyy")}_ANV_6966644";
            var bytes = Encoding.UTF8.GetBytes(token);
            var hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(token));
            string str = Convert.ToBase64String(hash);
            return str;
        }

        private B4.Modules.FileStorage.FileInfo GetArchive(long appcitId, string rsnumber)
        {
            try
            {
                var archive = new ZipFile(Encoding.UTF8)
                {
                    CompressionLevel = CompressionLevel.Level9,
                    AlternateEncoding = Encoding.GetEncoding("cp866"),
                    AlternateEncodingUsage = ZipOption.AsNecessary
                };

                var tempDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
             

                var attachments = AppealCitsAttachmentDomain.GetAll().Where(x => x.AppealCits.Id == appcitId).ToList();

                foreach (var file in attachments)
                {
                    System.IO.File.WriteAllBytes(
                        Path.Combine(tempDir.FullName, $"{file.FileInfo.Name}.{file.FileInfo.Extention}"),
                        FileManager.GetFile(file.FileInfo).ReadAllBytes());
                }

                archive.AddDirectory(tempDir.FullName);

                using (var ms = new MemoryStream())
                {
                    archive.Save(ms);

                    var file = FileManager.SaveFile(ms, $"{rsnumber}.zip");
                    return file;
                }
                /*
                var contentDisposition = new ContentDisposition();
                contentDisposition.Inline = false;
                this.Response.AddHeader("Content-Disposition", $@"attachment; filename={citizenSuggestion.Number} - {citizenSuggestion.ApplicantFio}.zip");
                var result = new FileStreamResult(ms, "application/zip");*/
            }
            finally
            {
            }
        }

        private bool ValidateToken(string check_token)
        {
            return this.GetAccessToken() == check_token;
        }

        private bool ValidateTokenPortal(string check_token)
        {
            var token = $"{DateTime.Now.ToString("dd.MM.yyyy")}_ANV_6966644";
            return token == check_token;
        }

        private string GetState(FuckingOSSState st, State state)
        {
            if (state.FinalState)
            {
                switch (st)
                {
                    case FuckingOSSState.No:
                        return "Мотивированный отказ";
                    case FuckingOSSState.NotSet:
                        return "Протокол ОСС не найден в архиве";
                    case FuckingOSSState.Work:
                        return "В работе";
                    case FuckingOSSState.Yes:
                        return "Предоставлена копия протокола ОСС";
                    default:
                        return "Поступила";
                }
            }
            else
            {
                switch (state.Name)
                {
                    case "Новое":
                        return "Поступила";
                    case "В работе":
                        return "В работе";
                    case "Проверка":
                        return "На проверке";
                    default:
                        return "Поступила";
                }
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
    }
}
