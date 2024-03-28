namespace Bars.GkhDi.DomainService
{
    using System;
    using System.Linq;

    using B4;
    using B4.Utils;
    using B4.DataAccess;
    using Entities;

    using Castle.Windsor;

    public class PlanWorkServiceRepairService : IPlanWorkServiceRepairService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddTemplateService(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                var service = Container.Resolve<IDomainService<PlanWorkServiceRepair>>();
                var serviceBaseService = Container.Resolve<IDomainService<BaseService>>();
                var serviceWorkRepList = Container.Resolve<IDomainService<WorkRepairList>>();
                var servicePlanWorkServiceRepairWorks = Container.Resolve<IDomainService<PlanWorkServiceRepairWorks>>();
                var serviceDisInfoRobject = Container.Resolve<IDomainService<DisclosureInfoRealityObj>>();

                try
                {
                    var disInfoRealityObjId = baseParams.Params.GetAs<long>("disclosureInfoRealityObjId");
                    var objectIdsString = baseParams.Params.GetAs("objectIds", string.Empty);

                    if (disInfoRealityObjId == 0 || string.IsNullOrEmpty(objectIdsString))
                    {
                        return new BaseDataResult { Success = false, Message = "Ошибка!Не удалось добавить план работ по содержанию и ремонту" };
                    }

                    var objectIds = objectIdsString.Contains(',') ? objectIdsString.Split(',').Select(x => x.ToLong()).ToArray() : new[] { objectIdsString.ToLong() };

                    var workRepairDict = serviceWorkRepList.GetAll()
                            .Where(x => x.BaseService.DisclosureInfoRealityObj.Id == disInfoRealityObjId)
                            .GroupBy(x => x.BaseService.Id)
                            .ToDictionary(x => x.Key);

                    // получаем добавленные услуги что бы не добавлять их повторно
                    var existRecs = 
                        service.GetAll()
                            .Where(x => x.DisclosureInfoRealityObj.Id == disInfoRealityObjId)
                            .Where(x => objectIds.Contains(x.BaseService.Id))
                            .Select(x => x.BaseService.Id)
                            .ToList();

                    var robject = serviceDisInfoRobject.Load(disInfoRealityObjId);

                    foreach (var id in objectIds)
                    {
                        if (existRecs.Contains(id))
                            continue;

                        var newRecord = new PlanWorkServiceRepair
                            {
                                BaseService = serviceBaseService.Load(id),
                                DisclosureInfoRealityObj = robject
                            };

                        // 1.Сохраняем запись плана работ по содержанию и ремонту
                        service.Save(newRecord);

                        if (workRepairDict.ContainsKey(id))
                        {
                            // 2.Получаем работы услуги ремонт из которой создана запись плана работ по содержанию и ремонту
                            // 3.Создаем работы на основе укрупненных(ППР) работ из услуги ремонт
                            foreach (var workRepairList in workRepairDict[id])
                            {
                                var newRecordWork = new PlanWorkServiceRepairWorks
                                    {
                                        PlanWorkServiceRepair = newRecord,
                                        WorkRepairList = workRepairList,
                                        Cost = workRepairList.PlannedCost,
                                        FactCost = workRepairList.FactCost,
                                        DateStart = workRepairList.DateStart,
                                        DateEnd = workRepairList.DateEnd
                                    };

                                servicePlanWorkServiceRepairWorks.Save(newRecordWork);
                            }
                        }
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException exc)
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false, Message = exc.Message };
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    Container.Release(service);
                    Container.Release(serviceWorkRepList);
                    Container.Release(serviceBaseService);
                    Container.Release(serviceDisInfoRobject);
                    Container.Release(servicePlanWorkServiceRepairWorks);
                }
            }
        }

        public IDataResult ReloadWorkRepairList(BaseParams baseParams)
        {
            try
            {
                var planWorkServiceRepairId = baseParams.Params.GetAs<long>("planWorkServiceRepairId");
                var service = Container.Resolve<IDomainService<PlanWorkServiceRepairWorks>>();

                // Получаем план работ по содержанию и ремонту
                var planWorkServiceRepair = Container.Resolve<IDomainService<PlanWorkServiceRepair>>().Get(planWorkServiceRepairId);

                // Получаем работы которые имеются у плана работ по содержанию и ремонту
                var worksOfPlan = 
                    service.GetAll()
                        .Where(x => x.PlanWorkServiceRepair.Id == planWorkServiceRepairId)
                        .ToList();

                // Получаем работы услуги из которой создан план
                var worksRepairService = 
                    Container.Resolve<IDomainService<WorkRepairList>>().GetAll()
                        .Where(x => x.BaseService.Id == planWorkServiceRepair.BaseService.Id)
                        .ToList();

                // Ищем работы которые есть в услуге ремонт, но нету в плане
                var notExistingWorks = worksRepairService.Where(x => !worksOfPlan.Select(y => y.WorkRepairList.Id).Distinct().Contains(x.Id)).ToList();

                // Добавляем работы которые есть в услуге но нету в плане
                if (notExistingWorks.Any())
                {
                    foreach (var notExistingWorksItem in notExistingWorks)
                    {
                        var work = new PlanWorkServiceRepairWorks
                        {
                            Id = 0,
                            PlanWorkServiceRepair = planWorkServiceRepair,
                            WorkRepairList = notExistingWorksItem,
                            Cost = notExistingWorksItem.PlannedCost,
                            FactCost = notExistingWorksItem.FactCost,
                            DateStart = notExistingWorksItem.DateStart,
                            DateEnd = notExistingWorksItem.DateEnd,
                        };

                        service.Save(work);
                    }
                }

                // Обновляем имеющиеся в плане работы
                foreach (var work in worksOfPlan)
                {
                    work.Cost = work.WorkRepairList.PlannedCost;
                    work.FactCost = work.WorkRepairList.FactCost;
                    work.DateStart = work.WorkRepairList.DateStart;
                    work.DateEnd = work.WorkRepairList.DateEnd;

                    service.Update(work);
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }
    }
}