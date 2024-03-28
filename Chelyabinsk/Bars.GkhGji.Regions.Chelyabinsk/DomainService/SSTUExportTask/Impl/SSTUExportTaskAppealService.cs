namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
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
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Bars.B4.Modules.States;

    public class SSTUExportTaskAppealService : ISSTUExportTaskAppealService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<AppealCits> AppealCitsDomain { get; set; }

        public IDomainService<AppealCitsSource> AppealCitsSourceDomain { get; set; }

        public IDomainService<AppealCitsStatSubject> AppealCitsStatSubjectDomain { get; set; }

        public IDomainService<SSTUExportTask> SSTUExportTaskDomain { get; set; }

        public IDomainService<SSTUExportTaskAppeal> SSTUExportTaskAppealDomain { get; set; }

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
            List<long> idList = new List<long> { 2, 201, 402, 1734, 1798, 1863, 1894, 1928, 3301, 3333, 3270, 3273, 3274, 3275 };

            Dictionary<QuestionStatus, string> dictStateQuestion = new Dictionary<QuestionStatus, string>();
            dictStateQuestion.Add(QuestionStatus.NotSet, "Не задано");
            dictStateQuestion.Add(QuestionStatus.Answered, "Дан ответ автору");
            dictStateQuestion.Add(QuestionStatus.Explained, "Рассмотрено. Разъяснено");
            dictStateQuestion.Add(QuestionStatus.InWork, "На рассмотрении");
            dictStateQuestion.Add(QuestionStatus.LeftWithoutAnswer, "Оставлено без ответа");
            dictStateQuestion.Add(QuestionStatus.NotSupported, "Не поддержано");
            dictStateQuestion.Add(QuestionStatus.Supported, "Поддержано");
            dictStateQuestion.Add(QuestionStatus.Transferred, "Перенаправлено");

            if (sSTUExportTask.SSTUSource == SSTUSource.Direct)
            {
                if (!sSTUExportTask.ExportExported)
                {
                    var data = AppealCitsSourceDomain.GetAll()
                        .Where(x => x.AppealCits != null && x.AppealCits.DateFrom.HasValue && x.AppealCits.NumberGji != "" && x.AppealCits.DateFrom > DateTime.Now.AddMonths(-4))
                        .Where(x => x.AppealCits.SSTUExportState == SSTUExportState.NotExported || x.AppealCits.SSTUExportState == SSTUExportState.Error)
                         // .Where(x => x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenHe || x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenShe || x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenThey)
                         .Where(x => x.RevenueSource != null && idList.Contains(x.RevenueSource.Id))
                        .Select(x => new
                        {
                            x.AppealCits.Id,
                            AppealNum = x.AppealCits.NumberGji,
                            AppealDate = x.AppealCits.DateFrom,
                            AppState = dictStateQuestion.ContainsKey(x.AppealCits.QuestionStatus) ? dictStateQuestion[x.AppealCits.QuestionStatus] : ""
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

                    var data = AppealCitsSourceDomain.GetAll()
                        .Where(x => exported.Contains(x.AppealCits.Id))
                        .Where(x => x.AppealCits != null && x.AppealCits.DateFrom.HasValue && x.AppealCits.NumberGji != "" && x.AppealCits.DateFrom > DateTime.Now.AddMonths(-4))
                        .Where(x => x.AppealCits.SSTUExportState == SSTUExportState.NotExported || x.AppealCits.SSTUExportState == SSTUExportState.Error)
                        //  .Where(x => x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenHe || x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenShe || x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenThey)
                        .Where(x => x.RevenueSource != null && idList.Contains(x.RevenueSource.Id))
                                          .Select(x => new
                                          {
                                              x.AppealCits.Id,
                                              AppealNum = x.AppealCits.NumberGji,
                                              AppealDate = x.AppealCits.DateFrom,
                                              AppState = dictStateQuestion.ContainsKey(x.AppealCits.QuestionStatus) ? dictStateQuestion[x.AppealCits.QuestionStatus] : ""
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
                         .Where(x => x.AppealCits != null && x.AppealCits.DateFrom.HasValue && x.AppealCits.NumberGji != "" && x.AppealCits.DateFrom > DateTime.Now.AddMonths(-3))
                         .Where(x => x.ExportCode != null && x.ExportCode.Length > 3)
                         .Select(x => x.AppealCits.Id).Distinct().ToList();


                    var data = AppealCitsSourceDomain.GetAll()
                  .Where(x => appChosenOneIds.Contains(x.AppealCits.Id))
                  .Where(x => x.AppealCits != null && x.AppealCits.DateFrom.HasValue && x.AppealCits.NumberGji != "" && x.AppealCits.DateFrom > DateTime.Now.AddMonths(-4))
                  .Where(x => x.AppealCits.SSTUExportState == SSTUExportState.NotExported || x.AppealCits.SSTUExportState == SSTUExportState.Error)
                   //    .Where(x => x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenHe || x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenShe || x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenThey)
                   .Where(x => x.RevenueSource != null && !idList.Contains(x.RevenueSource.Id))
                   .AsEnumerable()
                  .Select(x => new
                  {
                      x.AppealCits.Id,
                      AppealNum = x.RevenueSourceNumber,
                      AppealDate = x.AppealCits.DateFrom,
                      AppState = dictStateQuestion.ContainsKey(x.AppealCits.QuestionStatus) ? dictStateQuestion[x.AppealCits.QuestionStatus] : ""
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

                    var data = AppealCitsSourceDomain.GetAll()
                    .Where(x => exported.Contains(x.AppealCits.Id))
                    .Where(x => x.AppealCits != null && x.AppealCits.DateFrom.HasValue && x.AppealCits.NumberGji != "" && x.AppealCits.DateFrom > DateTime.Now.AddMonths(-4))
                    .Where(x => x.AppealCits.SSTUExportState == SSTUExportState.NotExported || x.AppealCits.SSTUExportState == SSTUExportState.Error)
                     //  .Where(x => x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenHe || x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenShe || x.AppealCits.TypeCorrespondent == TypeCorrespondent.CitizenThey)
                     .Where(x => x.RevenueSource != null && !idList.Contains(x.RevenueSource.Id))
                    .Select(x => new
                    {
                        x.AppealCits.Id,
                        AppealNum = x.RevenueSourceNumber,
                        AppealDate = x.AppealCits.DateFrom,
                        AppState = dictStateQuestion.ContainsKey(x.AppealCits.QuestionStatus) ? dictStateQuestion[x.AppealCits.QuestionStatus] : ""
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

        public IDomainService<AppealCitsExecutant> AppealCitsExecutantDomain { get; set; }

        public IDomainService<AppealCitsRealityObject> appealCitsRealityObjectDomain { get; set; }

        public IDomainService<ManOrgContractRealityObject> manOrgContractRealityObjectDomain { get; set; }

        public IDomainService<State> StateDomain { get; set; }

        public IDomainService<AppealOrder> AppealOrderDomain { get; set; }

        public virtual IDataResult ActualizeSopr(BaseParams baseParams)
        {
            string log = string.Empty;
            try
            {
                List<string> statSubjectCodes = new List<string> { "02", "04", "05" };

                var appealsToExportBySubject = AppealCitsStatSubjectDomain.GetAll()
                    .Where(x => x.AppealCits.State.StartState)
                    .Where(x => x.Subject != null)
                    .Where(x => (x.Subject.ISSOPR && x.Subsubject.ISSOPR) || (x.Subsubject == null && x.Subject.ISSOPR))
                    .Select(x => x.AppealCits.Id).Distinct()
                    .ToList();

                var appealsToExport = AppealCitsExecutantDomain.GetAll()
                    .Where(x => x.AppealCits.State.StartState)
                    .Where(x => appealsToExportBySubject.Contains(x.AppealCits.Id))
                    .Select(x => x.AppealCits.Id).Distinct().ToList();

                var appealRepo = this.Container.Resolve<IRepository<AppealCits>>();

                int creatd = 0;

                var stateSOPR = StateDomain.GetAll()
                    .FirstOrDefault(x => x.Code == "СОПР");

                if (stateSOPR == null)
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
                            continue;
                        }
                        if (appeal.OrderContragent == null)
                        {
                            var realityObject = appealCitsRealityObjectDomain.FirstOrDefault(x => x.AppealCits == appeal);
                            if (realityObject == null)
                            {
                                log += appeal.DocumentNumber + " Не указан контрагент ";
                                continue;

                            }
                            var contract = manOrgContractRealityObjectDomain.GetAll()
                                .Where(x => x.RealityObject == realityObject.RealityObject)
                                .Where(x => x.ManOrgContract.ManagingOrganization != null)
                                .Where(x => x.ManOrgContract.StartDate.HasValue && x.ManOrgContract.StartDate.Value <= DateTime.Now)
                                .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate.Value > DateTime.Now).FirstOrDefault();
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

                                creatd++;

                                appeal.State = stateSOPR;
                                appealRepo.Update(appeal);
                                try
                                {
                                    string email = contragent.Email;
                                    if (!string.IsNullOrEmpty(email))
                                    {
                                        try
                                        {
                                            EmailSender emailSender = EmailSender.Instance;
                                            emailSender.Send(email, "Уведомление о передаче на оперативное исполнение обращения", MakeMessageBody(appeal), null);
                                        }
                                        catch
                                        {
                                            log += appeal.DocumentNumber + " Не удалось отправить email ";
                                        }
                                    }

                                }
                                catch
                                {
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
        string MakeMessageBody(AppealCits appeal)
        {

            string body = $"Уважаемый(ая) пользователь!\r\n";
            body += $"Главное управление Государственная жилищная инспекция  Челябинской области сообщает Вам, что обращение {appeal.NumberGji} от {appeal.DateFrom.Value.ToShortDateString()} передано Вам для оперативного исполнения и размещено в реестре СОПР АИС ГЖИ Челябинской области.\r\n";
            return body;
        }
    }
}