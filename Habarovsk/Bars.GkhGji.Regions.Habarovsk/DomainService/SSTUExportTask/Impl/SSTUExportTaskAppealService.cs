namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Entities;
    using B4;
    using B4.DataAccess;
    using B4.Utils;


    using Castle.Windsor;
    using Bars.GkhGji.Regions.Habarovsk.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Bars.B4.Modules.States;
    using System.Net.Mail;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Application;

    public class SSTUExportTaskAppealService : ISSTUExportTaskAppealService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<AppealCits> AppealCitsDomain { get; set; }

        public IDomainService<AppealOrder> AppealOrderDomain { get; set; }

        public IDomainService<AppealCitsSource> AppealCitsSourceDomain { get; set; }

        public IDomainService<AppealCitsExecutant> AppealCitsExecutantDomain { get; set; }

        public IDomainService<AppealCitsStatSubject> AppealCitsStatSubjectDomain { get; set; }

        public IDomainService<AppealCitsRealityObject> appealCitsRealityObjectDomain { get; set; }

        public IDomainService<ManOrgContractRealityObject> manOrgContractRealityObjectDomain { get; set; }

        public IDomainService<SSTUExportTask> SSTUExportTaskDomain { get; set; }

        public IDomainService<SSTUExportTaskAppeal> SSTUExportTaskAppealDomain { get; set; }

        public IDomainService<State> StateDomain { get; set; }

        public IDomainService<SSTUExportedAppeal> SSTUExportedAppealDomain { get; set; }

        public IDataResult AddAppeal(BaseParams baseParams)
        {
            var taskId = baseParams.Params.ContainsKey("taskId") ? baseParams.Params["taskId"].ToLong() : 0;
            var appealIds = baseParams.Params.ContainsKey("appealIds") ? baseParams.Params["appealIds"].ToString() : "";
            var listIds = new List<long>();
            var task = SSTUExportTaskDomain.GetAll().FirstOrDefault(x => x.Id == taskId);

            if (task == null)
            {
                return new BaseDataResult(false, "Не удалось определить задачу по Id " + task.ToStr());
            }


            var gjiAppealIds = appealIds.Split(',').Select(id => id.ToLong()).ToList();

            listIds.AddRange(SSTUExportTaskAppealDomain.GetAll()
                                .Where(x => x.SSTUExportTask.Id == taskId)
                                .Select(x => x.AppealCits.Id)
                                .Distinct()
                                .ToList());

            foreach (var newId in gjiAppealIds)
            {

                // Если среди существующих документов уже есть такой документ то пролетаем мимо
                if (listIds.Contains(newId))
                    continue;

                // Если такого решения еще нет то добалвяем
                var newObj = new SSTUExportTaskAppeal();
                newObj.AppealCits = AppealCitsDomain.Get(newId);
                newObj.SSTUExportTask = task;
                newObj.ObjectVersion = 1;
                newObj.ObjectCreateDate = DateTime.Now;
                newObj.ObjectEditDate = DateTime.Now;

                SSTUExportTaskAppealDomain.Save(newObj);
            }

            return new BaseDataResult();
        }

        public IDataResult GetListExportableAppeals(BaseParams baseParams, bool isPaging, out int totalCount)
        {

            totalCount = 0;
            var sstuExportTaskId = baseParams.Params.GetAs<long>("sstuExportTaskId");
            var loadParams = baseParams.GetLoadParam();
            var sSTUExportTask = SSTUExportTaskDomain.Get(sstuExportTaskId);
            long eaisid = 3338;
            List<string> codeTransfer = new List<string> { "06","13", "15" };

            Dictionary<QuestionStatus, string> dictStateQuestion = new Dictionary<QuestionStatus, string>();
            dictStateQuestion.Add(QuestionStatus.NotSet, "Не задано");
            dictStateQuestion.Add(QuestionStatus.Answered, "Дан ответ автору");
            dictStateQuestion.Add(QuestionStatus.Explained, "Рассмотрено. Разъяснено");
            dictStateQuestion.Add(QuestionStatus.InWork, "На рассмотрении");
            dictStateQuestion.Add(QuestionStatus.LeftWithoutAnswer, "Оставлено без ответа");
            dictStateQuestion.Add(QuestionStatus.NotSupported, "Не поддержано");
            dictStateQuestion.Add(QuestionStatus.Supported, "Поддержано");
            dictStateQuestion.Add(QuestionStatus.Transferred, "Перенаправлено");
            dictStateQuestion.Add(QuestionStatus.Plizdu, "Рассмотрено. Поддержано. Меры приняты.");

            if (sSTUExportTask.SSTUSource == SSTUSource.Direct)
            {
                if (!sSTUExportTask.ExportExported)
                {
                    var data = AppealCitsDomain.GetAll()
                        .Where(x => x.DateFrom.HasValue && x.NumberGji != "" && x.DateFrom > DateTime.Now.AddMonths(-3))
                        .Where(x=> x.QuestionStatus != QuestionStatus.NotSet)
                        .Where(x => x.SSTUExportState == SSTUExportState.NotExported || x.SSTUExportState == SSTUExportState.Error)
                         // .Where(x => x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenHe || x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenShe || x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenThey)
                         .Where(x => x.KindStatement != null && !codeTransfer.Contains(x.KindStatement.Code))
                        .Select(x => new
                        {
                            x.Id,
                            AppealNum = x.DocumentNumber,
                            AppealDate = x.DateFrom,
                            AppState = dictStateQuestion.ContainsKey(x.QuestionStatus) ? dictStateQuestion[x.QuestionStatus] : ""
                        })
                         .AsQueryable()
                        .Filter(loadParams, Container);

                    totalCount = data.Count();

                    if (isPaging)
                    {
                        return new BaseDataResult(data.Order(loadParams).Paging(loadParams).ToList());
                    }

                    return new BaseDataResult(data.Order(loadParams).ToList());
                }
                else
                {
                    var exported = SSTUExportedAppealDomain.GetAll()
                        .Where(x => x.ObjectCreateDate > DateTime.Now.AddMonths(-3))
                        .Select(x => x.AppealCits.Id).ToList();

                    var data = AppealCitsDomain.GetAll()
                        .Where(x=> exported.Contains(x.Id))
                       .Where(x => x.DateFrom.HasValue && x.NumberGji != "" && x.DateFrom > DateTime.Now.AddMonths(-3))
                        .Where(x => x.QuestionStatus != QuestionStatus.NotSet)
                        .Where(x => x.SSTUExportState == SSTUExportState.NotExported || x.SSTUExportState == SSTUExportState.Error)
                         // .Where(x => x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenHe || x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenShe || x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenThey)
                         .Where(x => x.KindStatement != null && !codeTransfer.Contains(x.KindStatement.Code))
                        .Select(x => new
                        {
                            x.Id,
                            AppealNum = x.DocumentNumber,
                            AppealDate = x.DateFrom,
                            AppState = dictStateQuestion.ContainsKey(x.QuestionStatus) ? dictStateQuestion[x.QuestionStatus] : ""
                        })
                         .AsQueryable()
                        .Filter(loadParams, Container);

                    totalCount = data.Count();

                    if (isPaging)
                    {
                        return new BaseDataResult(data.Order(loadParams).Paging(loadParams).ToList());
                    }

                    return new BaseDataResult(data.Order(loadParams).ToList());
                }
            }
            else
            {
                if (!sSTUExportTask.ExportExported)
                {
                    var appChosenOneIds = AppealCitsStatSubjectDomain.GetAll()
                         .Where(x => x.AppealCits != null && x.AppealCits.DateFrom.HasValue && x.AppealCits.Number != "" && x.AppealCits.DateFrom > DateTime.Now.AddMonths(-3))
                         .Where(x => x.ExportCode != null && x.ExportCode.Length > 3)
                         .Select(x => x.AppealCits.Id).Distinct().ToList();
                    //    

                    var data = AppealCitsDomain.GetAll()
                        .Where(x => appChosenOneIds.Contains(x.Id))
                        .Where(x => x.DateFrom.HasValue && x.NumberGji != "" && x.DateFrom > DateTime.Now.AddMonths(-4))
                        .Where(x => x.SSTUExportState == SSTUExportState.NotExported || x.SSTUExportState == SSTUExportState.Error)
                         // .Where(x => x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenHe || x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenShe || x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenThey)
                         .Where(x => x.KindStatement != null && codeTransfer.Contains(x.KindStatement.Code))
                        .Select(x => new
                        {
                            x.Id,
                            AppealNum = x.DocumentNumber,
                            AppealDate = x.DateFrom,
                            AppState = dictStateQuestion.ContainsKey(x.QuestionStatus) ? dictStateQuestion[x.QuestionStatus] : ""
                        })
                         .AsQueryable()
                        .Filter(loadParams, Container);

                    totalCount = data.Count();

                    if (isPaging)
                    {
                        return new BaseDataResult(data.Order(loadParams).Paging(loadParams).ToList());
                    }

                    return new BaseDataResult(data.Order(loadParams).ToList());
                }
                else
                {
                    var exported = SSTUExportedAppealDomain.GetAll()
                       .Where(x => x.ObjectCreateDate > DateTime.Now.AddMonths(-3))
                       .Select(x => x.AppealCits.Id).ToList();

                    var data = AppealCitsDomain.GetAll()
                        .Where(x => exported.Contains(x.Id))
                        .Where(x => x.DateFrom.HasValue && x.NumberGji != "" && x.DateFrom > DateTime.Now.AddMonths(-4))
                        .Where(x => x.SSTUExportState == SSTUExportState.NotExported || x.SSTUExportState == SSTUExportState.Error)
                         // .Where(x => x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenHe || x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenShe || x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenThey)
                         .Where(x => x.KindStatement != null && codeTransfer.Contains(x.KindStatement.Code))
                        .Select(x => new
                        {
                            x.Id,
                            AppealNum = x.DocumentNumber,
                            AppealDate = x.DateFrom,
                            AppState = dictStateQuestion.ContainsKey(x.QuestionStatus) ? dictStateQuestion[x.QuestionStatus] : ""
                        })
                         .AsQueryable()
                        .Filter(loadParams, Container);

                    totalCount = data.Count();

                    if (isPaging)
                    {
                        return new BaseDataResult(data.Order(loadParams).Paging(loadParams).ToList());
                    }

                    return new BaseDataResult(data.Order(loadParams).ToList());
                }
            }
        }

        public virtual IDataResult ActualizeSopr(BaseParams baseParams)
        {
            string log = string.Empty;
            var appSettings = ApplicationContext.Current.Configuration.AppSettings;
            var min = appSettings.GetAs<bool>("MinimalApp");
            if (min)
            {
                return new BaseDataResult(true, $"Открытый сервер, обработка СОПР заблокирована");
            }
            try
            {
                List<string> statSubjectCodes = new List<string> { "02", "04","05" };

                var appealsToExportBySubject = AppealCitsStatSubjectDomain.GetAll()
                    .Where(x => x.AppealCits.State.StartState)
                    .Where(x => x.Subject != null)
                    .Where(x => (x.Subject.ISSOPR && x.Subsubject.ISSOPR)||(x.Subsubject == null && x.Subject.ISSOPR))
                    .Select(x=> x.AppealCits.Id).Distinct()
                    .ToList();

                var appealsToExport = AppealCitsExecutantDomain.GetAll()
                    .Where(x => x.AppealCits.State.StartState)
                    .Where(x => appealsToExportBySubject.Contains(x.AppealCits.Id))
                    .Select(x => x.AppealCits.Id).Distinct().ToList();

                var appealRepo = this.Container.Resolve<IRepository<AppealCits>>();

                int creatd = 0;

                var stateSOPR = StateDomain.GetAll()
                    .FirstOrDefault(x => x.Code == "СОПР");

                if(stateSOPR == null)
                return new BaseDataResult(false, $"Ошибка: Не найдет статус СОПР");

                foreach (long appId in appealsToExport)
                {
                    try
                    {

                        var appeal = AppealCitsDomain.Get(appId);
                        var existingOrder = AppealOrderDomain.GetAll()
                            .FirstOrDefault(x => x.AppealCits == appeal);
                        Contragent contragent = null;

                        if (existingOrder != null)
                        {
                            log += appeal.DocumentNumber + " Уведомление уже создано. Повторное уведомление запрещено. ";
                            try
                            {
                                if (appeal.State.Code != "СОПР")
                                {
                                    if (appeal.State.StartState)
                                    {
                                        appeal.State = stateSOPR;
                                        AppealCitsDomain.Update(appeal);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                return new BaseDataResult(false, $"Ошибка: {e.InnerException}" + log);
                            }
                            continue;
                        }
                        if (appeal.OrderContragent == null)
                        {
                            ManOrgContractRealityObject contract = null;
                            var realityObject = appealCitsRealityObjectDomain.FirstOrDefault(x => x.AppealCits == appeal);
                            if (realityObject == null)
                            {
                                log += appeal.DocumentNumber + " Не указан контрагент ";
    
                            }
                            else
                            {
                                contract = manOrgContractRealityObjectDomain.GetAll()
                                    .Where(x => x.RealityObject == realityObject.RealityObject)
                                    .Where(x => x.ManOrgContract.ManagingOrganization != null)
                                    .Where(x => x.ManOrgContract.StartDate.HasValue && x.ManOrgContract.StartDate.Value <= DateTime.Now)
                                    .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate.Value > DateTime.Now).FirstOrDefault();
                            }
                            if (contract == null)
                            {
                                log += appeal.DocumentNumber + " Не указан контрагент ";
                            }
                            else if (contract.ManOrgContract.ManagingOrganization != null)
                            {
                                contragent = contract.ManOrgContract.ManagingOrganization.Contragent;
                            }
                            else
                            {
                                log += appeal.DocumentNumber + " Не указан контрагент ";
                            }


                        }
                        else
                        {
                            contragent = appeal.OrderContragent;
                        }
                        if (contragent == null)
                        {
                            log += appeal.DocumentNumber + " Не удалось определить контрагента ";
                        }
                        else
                        {
                            if (contragent.IsSOPR)
                            {
                                AppealOrder newOrder = new AppealOrder
                                {
                                    AppealCits = new AppealCits { Id = appeal.Id },
                                    AppealText = appeal.Description,
                                    Executant = contragent,
                                    OrderDate = DateTime.Now,
                                    YesNoNotSet = Gkh.Enums.YesNoNotSet.No,
                                    Confirmed = Gkh.Enums.YesNoNotSet.NotSet
                                };
                                AppealOrderDomain.Save(newOrder);
                                var appealCitsRepo = Container.Resolve<IRepository<AppealCits>>();
                                var appealCits = appealCitsRepo.Get(appeal.Id);
                                appealCits.CaseDate = DateTime.Now;

                                creatd++;

                                appeal.State = stateSOPR;

                                //запиливаем новый контрольный срок
                                DateTime newControlDate = DateTime.Now;                              
                                newControlDate = appeal.DateFrom.Value.AddDays(9);
                                var prodCalendarContainer = this.Container.Resolve<IDomainService<ProdCalendar>>().GetAll()
                    .Where(x => x.ProdDate >= appeal.DateFrom.Value && x.ProdDate <= appeal.DateFrom.Value.AddDays(9)).Select(x => x.ProdDate).ToList();

                                if (prodCalendarContainer.Contains(newControlDate))
                                {
                                    for (int i = 0; i <= prodCalendarContainer.Count; i++)
                                    {
                                        if (prodCalendarContainer.Contains(newControlDate))
                                        {
                                            newControlDate = newControlDate.AddDays(-1);
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }

                                if (newControlDate.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    newControlDate = newControlDate.AddDays(-1);
                                }
                                else if (newControlDate.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    newControlDate = newControlDate.AddDays(-2);
                                }


                                appeal.CaseDate = newControlDate;
                                appealRepo.Update(appeal);
                                try {
                                    string email = contragent.Email;
                                    if (!string.IsNullOrEmpty(email))
                                    {
                                        try
                                        {
                                            EmailSender emailSender = EmailSender.Instance;
                                            emailSender.Send(email, "Уведомление о размещении документа ГЖИ", MakeMessageBody(), null);
                                        }
                                        catch
                                        {
                                            log += appeal.DocumentNumber + " Не удалось отправить email ";
                                        }
                                    }

                                }
                                catch {
                                    log += appeal.DocumentNumber + " Не удалось отправить email ";
                                }
                            }
                            else
                            {
                                log += appeal.DocumentNumber + " Контрагент не является участником электронного обмена ";
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        log += appId + " Не удалось создать уведомление для УК " + e.Message;
                    }
                }            

                log += "открытых обращений обращений найдено " + appealsToExportBySubject.Count + " ";
                log += "из них с назначенным исполнителем " + appealsToExport.Count + " ";




                return new BaseDataResult(true, $"Отправка обращений в СОПР завершена. Отправлено: {creatd} " + log);
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, $"Ошибка: {e.InnerException}" + log);
            }


        }

        string MakeMessageBody()
        {
           
            string body = $"Уважаемый(ая) пользователь!\r\n";
            body += $"Государственная жилищная инспекция Воронежской области уведомляет Вас о том, что вам поступило новое обращение в реестре СОПР.\r\n";
            body += $"Данный почтовый адрес используется для автоматического уведомления пользователей системы электронного документооборота и не предназначен для приема какого-либо рода электронных сообщений (обращений)";
            return body;
        }
    }
}