namespace Bars.GkhCr.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using System.Security.Cryptography;
    using System.Text;
    using Bars.B4;
    using Bars.Gkh.Entities; 
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Services.DataContracts;
    using Bars.B4.Modules.Security;
    using Bars.Gkh.Entities.Dicts;
    using Bars.B4.Modules.States;
    // TODO : Найти аналогичную библиотеку
   // using GeoCoordinatePortable;

    /// <summary>
    /// Сервис сведений об обращениях граждан
    /// </summary>
    public partial class Service
    {
        public IDomainService<RealityObjectHousekeeper> HousekeeperDomain { get; set; }
        public IDomainService<HousekeeperReport> HousekeeperReportDomain { get; set; }
        public IDomainService<ObjectCr> ObjectCrDomain { get; set; }
        public IDomainService<State> StateDomain { get; set; }
        public IDomainService<ZonalInspection> ZonalInspectionDomain { get; set; }
        public IDomainService<ZonalInspectionInspector> ZonalInspectionInspectorDomain { get; set; }
        public IDomainService<Inspector> InspectorDomain { get; set; }
        public IDomainService<BuildContractTypeWork> BuildContractTypeWorkDomain { get; set; }
        public IDomainService<BuildContract> BuildContractDomain { get; set; }
        public IDomainService<BuildControlTypeWorkSmr> BuildControlTypeWorkSmrDomain { get; set; }
        public IDomainService<BuildControlTypeWorkSmrFile> BuildControlTypeWorkSmrFileDomain { get; set; }
        public IDomainService<AdditWork> AdditWorkDomain { get; set; }
        public IDomainService<TypeWorkCr> TypeWorkCrDomain { get; set; }
        public IDomainService<TypeWorkCrAddWork> TypeWorkCrAddWorkDomain { get; set; }
        public IDomainService<HousekeeperReportFile> HousekeeperReportFileDomain { get; set; }
        public IDomainService<Operator> OperatorDomain { get; set; }
        public IDomainService<OperatorContragent> OperatorContragentDomain { get; set; }
        public IDomainService<UserPassword> UserPasswordDomain { get; set; }

        public InsertHousekeeperReportResponce InsertHousekeeperReport(HousekeeperReportProxy report, string token)
        {
            if (!ValidateToken(token))
            {
                return new InsertHousekeeperReportResponce
                {
                    Result = Result.IncorrectToken
                };
            }
            var objectCr = ObjectCrDomain.Get(report.ObjectCR);
            if (objectCr == null)
            {
                return new InsertHousekeeperReportResponce
                {
                    Result = Result.DataNotFound
                };
            }
            var housekeeper = HousekeeperDomain.GetAll()
                .FirstOrDefault(x => x.Login == report.HousekeeperLogin);
            if (housekeeper == null)
            {
                return new InsertHousekeeperReportResponce
                {
                    Result = Result.DataNotFound
                };
            }
            HousekeeperReport newReport = new HousekeeperReport
            {
               Description = report.Description,
               IsArranged = false,
               ObjectCr = objectCr,
               ObjectCreateDate = DateTime.Now,
               ObjectEditDate = DateTime.Now,
               ObjectVersion = 1,
               RealityObjectHousekeeper = housekeeper,
               ReportDate = report.ReportDate,
               ReportNumber = report.ReportNumber
            };
            try
            {
                HousekeeperReportDomain.Save(newReport);
                if(report.ReportFiles != null)
                foreach (var file in report.ReportFiles.ToList())
                {
                    var fromBase64 = Convert.FromBase64String(file.Base64);
                    var fileExt = GetNameAndExtention(file.FileName);
                    HousekeeperReportFile newReportFile = new HousekeeperReportFile
                    {
                        Description = file.Description,
                        HousekeeperReport = newReport,
                        FileInfo = FileManager.SaveFile(new FileData(fileExt[0], fileExt[1], fromBase64))
                    };
                    HousekeeperReportFileDomain.Save(newReportFile);
                }
                //Отправляем мыло сотрудникам
                try
                {
                    ZonalInspectionDomain.GetAll()
                        .Where(x => x.ShortName == "СК")
                        .Select(x => x.Id).ToList().ForEach(id =>
                        {
                            ZonalInspectionInspectorDomain.GetAll()
                        .Where(x => x.ZonalInspection.Id == id && x.Inspector.IsHead)
                        .Select(x => x.Inspector.Email).ToList().ForEach(email =>
                             {
                                 if (!string.IsNullOrEmpty(email))
                                 {
                                     Send(email, newReport);

                                  }
                             });
                        });

                    var builderMails = BuildContractService.GetAll()
                        .Where(x => x.ObjectCr == objectCr)
                        .Select(x => x.Builder.Contragent.Email).ToList();

                    foreach (string email in builderMails)
                    {
                        if (!string.IsNullOrEmpty(email))
                        {
                            Send(email, newReport);
                        }
                    };

                }
                catch
                {

                }
                return new InsertHousekeeperReportResponce
                {
                    Result = Result.NoErrors                    
                };
            }
            catch(Exception e)
            {
                return new InsertHousekeeperReportResponce
                {
                    Result = new Result
                    {
                        Code = "500",
                        Name = e.Message
                    }
                };
            }

        }

        public InsertBuildControlReportResponce InsertBuildControlReport(BuildControlReportProxy report, string token)
        {
            if (!ValidateToken(token))
            {
                return new InsertBuildControlReportResponce
                {
                    Result = Result.IncorrectToken
                };
            }
            var workStage = TypeWorkCrAddWorkDomain.Get(report.StageId);

            if (workStage == null)
            {
                return new InsertBuildControlReportResponce
                {
                    Result = Result.DataNotFound
                };
            }
            var objectCr = workStage.TypeWorkCr?.ObjectCr;
            if (objectCr != null)
            {
                var realityObject = objectCr.RealityObject;
                if (realityObject.Latitude.HasValue && realityObject.Longitude.HasValue)
                {
                    if (report.Latitude > 0 && report.Longitude > 0)
                    {
                        // TODO: Найти аналог
                        /* var newGeo = new GeoCoordinate((double)report.Latitude, (double)report.Longitude);
                        var dist = newGeo.GetDistanceTo(new GeoCoordinate((double)realityObject.Latitude.Value, (double)realityObject.Longitude.Value));
                        if (dist > 200)
                        {
                            return new InsertBuildControlReportResponce
                            {
                                Result = Result.NotClose
                            };                           
                        }*/
                    }
                }
            }

            var stateBKSMr = StateDomain.GetAll()
                   .Where(x => x.TypeId == "build_control_type_work_smr")
                .FirstOrDefault(x => x.StartState);

            BuildControlTypeWorkSmr newReport = new BuildControlTypeWorkSmr
            {
                Description = report.Description,
                Contragent = new Contragent {Id = report.ContragentSMRId },
                Controller = new Contragent { Id = report.ContragentSKId },
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                Latitude = report.Latitude,
                Longitude = report.Longitude,
                DeadlineMissed = report.DeadlineMissed,
                MonitoringDate = DateTime.Now,
                PercentOfCompletion = report.PercentOfCompletion,
                State = stateBKSMr,
                TypeWorkCr = workStage.TypeWorkCr,
                TypeWorkCrAddWork = workStage                
            };
            try
            {
                BuildControlTypeWorkSmrDomain.Save(newReport);
                if (report.ReportFiles != null)
                    foreach (var file in report.ReportFiles.ToList())
                    {
                        var fromBase64 = Convert.FromBase64String(file.Base64);
                        var fileExt = GetNameAndExtention(file.FileName);
                        BuildControlTypeWorkSmrFile newReportFile = new BuildControlTypeWorkSmrFile
                        {
                            Description = file.Description,
                            BuildControlTypeWorkSmr = newReport,
                            FileInfo = FileManager.SaveFile(new FileData(fileExt[0], fileExt[1], fromBase64))
                        };
                        BuildControlTypeWorkSmrFileDomain.Save(newReportFile);
                    }               
           
                return new InsertBuildControlReportResponce
                {
                    Result = Result.NoErrors
                };
            }
            catch (Exception e)
            {
                return new InsertBuildControlReportResponce
                {
                    Result = new Result
                    {
                        Code = "500",
                        Name = e.Message
                    }
                };
            }

        }

        /// <summary>
        /// Импорт сведений об обращении граждан
        /// </summary>
        /// <returns></returns>
        public GetHousekeeperReportResponce GetHousekeeperReport(string Login, string token)
        {
            if (!ValidateToken(token))
            {
                return new GetHousekeeperReportResponce
                {
                    HousekeeperReports = null,
                    Result = Result.IncorrectToken
                };
            }

            var housekeeper = HousekeeperDomain.GetAll()
               .FirstOrDefault(x => x.Login == Login);
            if (housekeeper == null)
            {
                return new GetHousekeeperReportResponce
                {
                    HousekeeperReports = null,
                    Result = Result.DataNotFound
                };
            }
            var reports = HousekeeperReportDomain.GetAll()
                .Where(x => x.RealityObjectHousekeeper != null && x.RealityObjectHousekeeper == housekeeper)
                .ToList();
            List<HousekeeperReportProxy> listReport = new List<HousekeeperReportProxy>();
            foreach (var report in reports)
            {
                listReport.Add(new HousekeeperReportProxy
                {
                    Description = report.Description,
                    HousekeeperLogin = report.RealityObjectHousekeeper.Login,
                    IsArranged = report.IsArranged,
                    ObjectCR = report.ObjectCr.Id,
                    ReportDate = report.ReportDate,
                    ReportNumber = report.ReportNumber,
                    Answer = report.Answer,
                    CheckDate = report.CheckDate.HasValue? report.CheckDate.Value.ToShortDateString():"",
                    CheckTime = report.CheckTime,
                    State = report.State.Name
                });
            }           

            return new GetHousekeeperReportResponce
            {
                HousekeeperReports = listReport.ToArray(),
                Result = Result.NoErrors
            };


        }

        /// <summary>
        /// Проверка пользователя стройконтроля
        /// </summary>
        /// <returns></returns>
        public CheckBuildControlUserResponce CheckBuildControlUser(string Login, string PasswordHash, string token)
        {
            if (!ValidateToken(token))
            {
                return new CheckBuildControlUserResponce
                {
                    ContragentId = 0,
                    UserId = 0,
                    Result = Result.IncorrectToken
                };
            }

            var operatorBK = OperatorDomain.GetAll()
                .Where(x=> x.IsActive && x.User.Login == Login)
               .Select(x => new
               {
                   x.Id,
                   UserId = x.User.Id,
                   x.User.Login,
                   x.User.Name,
                   x.ObjectEditDate
               }).FirstOrDefault();

            if (operatorBK == null)
            {
                return new CheckBuildControlUserResponce
                {
                    ContragentId=0,
                    UserId = 0,
                    Result = Result.DataNotFound
                };
            }
            var userInfo = UserPasswordDomain.Get(operatorBK.UserId);   

            if (userInfo == null)
            {
                return new CheckBuildControlUserResponce
                {
                    ContragentId = 0,
                    UserId = 0,
                    Result = Result.DataNotFound
                };
            }
            if (userInfo.Password != PasswordHash)
            {
                return new CheckBuildControlUserResponce
                {
                    ContragentId = 0,
                    UserId = userInfo.Id,
                    Result = Result.IncorrectPassword
                };
            }
            var operatorContragent = OperatorContragentDomain.GetAll()
                .Where(x => x.Operator != null)
                .Where(x => x.Operator.Id == operatorBK.Id)
                .FirstOrDefault();
            if (operatorContragent == null || operatorContragent.Contragent == null)
            {
                return new CheckBuildControlUserResponce
                {
                    ContragentId = 0,
                    UserId = userInfo.Id,
                    Result = Result.NoErrors
                };
            }


            return new CheckBuildControlUserResponce
            {
                ContragentId = operatorContragent.Contragent.Id,
                UserId = userInfo.Id,
                Result = Result.NoErrors
            };


        }

        /// <summary>
        /// Возвращаем список объектов КР для пользователя стройконтроля
        /// </summary>
        /// <returns></returns>
        public GetBuildControlObjectsResponce GetBuildControlObjects(long userId, string token)
        {
            if (!ValidateToken(token))
            {
                return new GetBuildControlObjectsResponce
                {
                    Objects = null,
                    Result = Result.IncorrectToken
                };
            }

            var operatorBK = OperatorDomain.GetAll()
                .Where(x=> x.IsActive && x.User.Id == userId)
               .Select(x => new
               {
                   x.Id,
                   UserId = x.User.Id,
                   x.User.Login,
                   x.User.Name,
                   x.ObjectEditDate
               }).FirstOrDefault();

            if (operatorBK == null)
            {
                return new GetBuildControlObjectsResponce
                {
                    Objects = null,
                    Result = Result.DataNotFound
                };
            }
            var operatorContragent = OperatorContragentDomain.GetAll()
                .Where(x => x.Operator != null)
                .Where(x => x.Operator.Id == operatorBK.Id)
                .FirstOrDefault();
            if (operatorContragent == null || operatorContragent.Contragent == null)
            {
                return new GetBuildControlObjectsResponce
                {
                    Objects = null,
                    Result = Result.DataNotFound
                };
            }
            //получаем объекты КР в которых есть утвержденный договор с подрядчиком который указан в учетке пользователя

            List<BuildControlObjectCR> listObjects = GetObjectCrBK(operatorContragent.Contragent.Id);

            if (listObjects.Count > 0)
            {
                return new GetBuildControlObjectsResponce
                {
                    Objects = listObjects.ToArray(),
                    Result = Result.NoErrors
                };
            }
            return new GetBuildControlObjectsResponce
            {
                Objects = null,
                Result = Result.DataNotFound
            };
        }

        /// <summary>
        /// Возвращаем список объектов КР для пользователя стройконтроля
        /// </summary>
        /// <returns></returns>
        public GetBuildControlObjectResponce GetBuildControlObject(long objectId, string token)
        {
            if (!ValidateToken(token))
            {
                return new GetBuildControlObjectResponce
                {
                    BuildControlObjectCR = null,
                    Result = Result.IncorrectToken
                };
            }

            //получаем объект КР по ид

            BuildControlObjectCR bkObject = GetObjectBK(objectId);

            if (bkObject == null)
            {
                return new GetBuildControlObjectResponce
                {
                    BuildControlObjectCR = null,
                    Result = Result.DataNotFound
                };
            }
            return new GetBuildControlObjectResponce
            {
                BuildControlObjectCR = bkObject,
                Result = Result.NoErrors
            };
        }

        /// <summary>
        /// Возвращаем список объектов КР для пользователя стройконтроля
        /// </summary>
        /// <returns></returns>
        public GetBuildControlReportListResponce GetBuildControlReportList(long objectId, string token)
        {
            if (!ValidateToken(token))
            {
                return new GetBuildControlReportListResponce
                {
                    BuildControlReportList = null,
                    Result = Result.IncorrectToken
                };
            }

            //получаем объект КР по ид
            List<BuildControlReportListItemProxy> bkObjectReport = GetObjectBKReport(objectId);

            if (bkObjectReport == null || bkObjectReport.Count==0)
            {
                return new GetBuildControlReportListResponce
                {
                    BuildControlReportList = null,
                    Result = Result.DataNotFound
                };
            }
            return new GetBuildControlReportListResponce
            {
                BuildControlReportList = bkObjectReport.ToArray(),
                Result = Result.NoErrors
            };
        }


        /// <summary>
        /// Импорт сведений об обращении граждан по объекту капитального ремонта
        /// </summary>
        /// <returns></returns>
        public GetHousekeeperReportResponce GetCrObjectReport(long crId, string token)
        {
            if (!ValidateToken(token))
            {
                return new GetHousekeeperReportResponce
                {
                    HousekeeperReports = null,
                    Result = Result.IncorrectToken
                };
            }

            
            var reports = HousekeeperReportDomain.GetAll()
                .Where(x => x.RealityObjectHousekeeper != null && x.ObjectCr.Id == crId)
                .ToList();
            List<HousekeeperReportProxy> listReport = new List<HousekeeperReportProxy>();
            foreach (var report in reports)
            {
                listReport.Add(new HousekeeperReportProxy
                {
                    Description = report.Description,
                    HousekeeperLogin = report.RealityObjectHousekeeper.Login,
                    IsArranged = report.IsArranged,
                    ObjectCR = report.ObjectCr.Id,
                    ReportDate = report.ReportDate,
                    ReportNumber = report.ReportNumber,
                    Answer = report.Answer,
                    CheckDate = report.CheckDate.HasValue? report.CheckDate.Value.ToShortDateString():"",
                    CheckTime = report.CheckTime,
                    State = report.State.Name
                });
            }
            return new GetHousekeeperReportResponce
            {
                HousekeeperReports = listReport.ToArray(),
                Result = Result.NoErrors
            };


        }
        /// <summary>
        /// Импорт сведений об обращении граждан
        /// </summary>
        /// <returns></returns>
        public ObjectCRMobileResponce GetObjectsCRMobile(long RealityId, string token)
        {
            if (!ValidateToken(token))
            {
                return new ObjectCRMobileResponce
                {
                    ObjectsCRMobile = null,
                    Result = Result.IncorrectToken
                };
            }
            List<ObjectCRMobile> objects = new List<ObjectCRMobile>();
            ObjectCrDomain.GetAll()
               .Where(x => x.RealityObject.Id == RealityId)
               .Where(x=> x.ProgramCr.TypeVisibilityProgramCr == Enums.TypeVisibilityProgramCr.Full)
               .ToList().ForEach(x =>
               {
                   List<CRObjectWork> works = new List<CRObjectWork>();
                   BuildContractTypeWorkDomain.GetAll()
                   .Where(z => z.BuildContract.ObjectCr == x)
                   .ToList().ForEach(z =>
                   {
                       works.Add(new CRObjectWork
                       {
                           ContractNumber = z.BuildContract.DocumentNum,
                           ContractState = z.BuildContract.State != null? z.BuildContract.State.Name:"Нет сведений",
                           WorkId = z.TypeWork.Id,
                           WorkName = z.TypeWork.Work.Name,
                           DateFrom = z.BuildContract.DateStartWork.HasValue ? z.BuildContract.DateStartWork.Value : DateTime.Now,
                           DateTo = z.BuildContract.DateEndWork.HasValue ? z.BuildContract.DateEndWork.Value : DateTime.Now
                       });
                   });
                   objects.Add(new ObjectCRMobile
                   {
                       ObjectId = x.Id,
                       Period = x.ProgramCr.Period !=null? x.ProgramCr.Period.Name:"Не указан",
                       ProgramName = x.ProgramCr.Name,
                       Works = works.ToArray()
                   });;

               });



            if (objects.Count ==0)
            {
                return new ObjectCRMobileResponce
                {
                    ObjectsCRMobile = null,
                    Result = Result.DataNotFound
                };
            }

            return new ObjectCRMobileResponce
            {
                ObjectsCRMobile = objects.ToArray(),
                Result = Result.NoErrors
            };


        }

        private void Send(String toEmail, HousekeeperReport entity)
        {
            var appSettings = ApplicationContext.Current.Configuration.AppSettings;
            var smtpClient = appSettings.GetAs<string>("smtpClient");
            var smtpPort = appSettings.GetAs<int>("smtpPort");
            var mailFrom = appSettings.GetAs<string>("smtpEmail");
            var smtpLogin = appSettings.GetAs<string>("smtpLogin");
            var smtpPassword = appSettings.GetAs<string>("smtpPassword");
            var enableSsl = appSettings.GetAs<bool>("enableSsl");

            try
            {
                MailAddress from = new MailAddress(mailFrom, "Система Барс.ЖКХ");
                MailAddress to = new MailAddress(toEmail);
                MailMessage m = new MailMessage(from, to);
                m.Subject = "Уведомление о поступлении обращения от старшего по дому";
                m.Body = $"<h3>Система уведомляет вас о поступлении обращения от старшего по дому. Адрес МКД {entity.ObjectCr.RealityObject.Municipality.Name}, {entity.ObjectCr.RealityObject.Address} в краткосрочной программе {entity.ObjectCr.ProgramCr.Name}</h3>\r\n";
                m.Body += $"Содержание обращения: {entity.Description}";
                m.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(smtpClient, smtpPort);
                smtp.Credentials = new NetworkCredential(smtpLogin, smtpPassword);
                smtp.EnableSsl = enableSsl;
                smtp.Send(m);
            }
            catch (Exception e)
            {

            }
        }
        private string GetAccessToken()
        {
            var token = $"{DateTime.Now.ToString("dd.MM.yyyy")}_ANV_6966644";
            var hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(token));
            string str = Convert.ToBase64String(hash);
            return Convert.ToBase64String(hash);
        }

        private bool ValidateToken(string check_token)
        {
            return this.GetAccessToken() == check_token;
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

        private List<BuildControlObjectCR> GetObjectCrBK(long contragentId)
        {
            var contractState = StateDomain.GetAll()
                .Where(x => x.TypeId == "cr_obj_build_contract")
                .FirstOrDefault(x => x.Name == "Утвержден");
            var crObjects = BuildContractDomain.GetAll()
                .Where(x => x.Builder.Contragent.Id == contragentId && x.State == contractState)
                .Where(x=> x.ObjectCr.ProgramCr.TypeVisibilityProgramCr == Enums.TypeVisibilityProgramCr.Full)
                .Where(x=> !x.DateEndWork.HasValue || x.DateEndWork.Value>DateTime.Now.AddMonths(-1))
                .Select(x=> new BuildControlObjectCR
                {
                    Address = $"{x.ObjectCr.RealityObject.Municipality.Name}, {x.ObjectCr.RealityObject.Address}",
                    ObjectId = x.ObjectCr.Id,
                    Period = x.ObjectCr.ProgramCr.Period != null ? x.ObjectCr.ProgramCr.Period.Name : "Не указан",
                    ProgramName = x.ObjectCr.ProgramCr.Name
                }).ToList();
            return crObjects;
        }

        private BuildControlObjectCR GetObjectBK(long objectId)
        {
            var contractState = StateDomain.GetAll()
                .Where(x => x.TypeId == "cr_obj_build_contract")
                .FirstOrDefault(x => x.Name == "Утвержден");

            var typeworks = TypeWorkCrDomain.GetAll()
                .Where(x => x.ObjectCr.Id == objectId)
                .Where(x => x.DateEndWork.HasValue && x.DateEndWork.Value > DateTime.Now.AddMonths(-2))
                .Select(x => new TypeWorkCr
                {
                    Id = x.Id,
                    Work = new Work { Id = x.Work.Id, Name = x.Work.Name },
                    DateStartWork = x.DateStartWork,
                    DateEndWork = x.DateEndWork,
                    PercentOfCompletion = x.PercentOfCompletion.HasValue ? x.PercentOfCompletion : 0

                }).ToList();
            List<CRObjectWorkSmr> works = new List<CRObjectWorkSmr>();
            foreach (TypeWorkCr work in typeworks)
            {
                List<CRObjectWorkStage> stages = new List<CRObjectWorkStage>();
                var buildContr = BuildContractTypeWorkDomain.GetAll()
                    .Where(x => x.TypeWork.Id == work.Id && x.BuildContract.State == contractState)
                    .Select(x => x.BuildContract).FirstOrDefault();
                if (buildContr == null)
                {
                    continue;
                }
                stages.AddRange(TypeWorkCrAddWorkDomain.GetAll()
                    .Where(x=> x.TypeWorkCr.Id == work.Id)
                    .Select(x=> new CRObjectWorkStage
                    {
                        StageId = x.Id,
                        WorkId = x.TypeWorkCr.Id,
                        WorkName = x.AdditWork.Name,
                        DateStartWork = x.DateStartWork,
                        DateEndWork = x.DateEndWork
                    }).OrderBy(x=> x.DateStartWork).ToList());
                if(stages.Count>0)
                {
                    works.Add(new CRObjectWorkSmr
                    {
                        ContractNumber = buildContr.DocumentNum,
                        ContractState = buildContr.State.Name,
                        ContragentId = buildContr.Builder != null? buildContr.Builder.Contragent.Id:0,
                        ContragentName = buildContr.Builder != null ? buildContr.Builder.Contragent.Name : "",
                        DateFrom = work.DateStartWork.HasValue? work.DateStartWork.Value: buildContr.DateStartWork.Value,
                        DateTo = work.DateEndWork.HasValue ? work.DateEndWork.Value : buildContr.DateEndWork.Value,
                        WorkId = work.Id,
                        WorkName = work.Work.Name,
                        WorkStages = stages.ToArray()
                    });
                }

            }
            if (works.Count > 0)
            {
                var objCr = ObjectCrDomain.Get(objectId);
                if (objCr != null)
                {
                    return new BuildControlObjectCR
                    {
                        Address = $"{objCr.RealityObject.Municipality.Name}, {objCr.RealityObject.Address}",
                        ObjectId = objCr.Id,
                        Period = objCr.ProgramCr.Period != null ? objCr.ProgramCr.Period.Name : "Не указан",
                        ProgramName = objCr.ProgramCr.Name,
                        Works = works.ToArray()
                    };
                }                
            }
            return new BuildControlObjectCR();
        }

        private List<BuildControlReportListItemProxy> GetObjectBKReport(long objectId)
        {

            var typeworks = BuildControlTypeWorkSmrDomain.GetAll()
                .Where(x => x.TypeWorkCr.ObjectCr.Id == objectId)
                .Select(x => new BuildControlReportListItemProxy
                {
                    Id = x.Id,
                    WorkName = x.TypeWorkCr.Work.Name,
                    ContragentSMR = x.Contragent != null? x.Contragent.Name:"Не указан",
                    ContragentSK = x.Controller != null? x.Controller.Name : "Не указан",
                    DeadlineMissed = x.DeadlineMissed,
                    Description = x.Description,
                    ObjectCR = $"{x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Name}, {x.TypeWorkCr.ObjectCr.RealityObject.Address}",
                    StageName = x.TypeWorkCrAddWork.AdditWork.Name
                }).ToList();
         
            return typeworks;
        }
    }
}