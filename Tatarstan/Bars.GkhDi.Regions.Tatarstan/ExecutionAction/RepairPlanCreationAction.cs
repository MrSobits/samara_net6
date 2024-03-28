namespace Bars.GkhDi.Regions.Tatarstan.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;
    using Bars.GkhDi.Regions.Tatarstan.Entities;

    public class RepairPlanCreationAction : BaseExecutionAction
    {
        public IRepository<RepairService> RepairServiceRepository { get; set; }

        public IRepository<WorkRepairListTat> WorkRepairListTatRepository { get; set; }

        public IDomainService<PlanWorkServiceRepair> PlanWorkServiceRepairDomainService { get; set; }

        public IDomainService<PlanWorkServiceRepairWorks> PlanWorkServiceRepairWorksDomainService { get; set; }

        public override string Description
            => @"По всем организациям по каждой услуге с видом='ремонт' И 'тип предоставления услуги'= 'услуга предоставляется через УО' 
1. Создает запись в разделе 'план работ по содержанию и ремонту' 
2. Переносит туда соответствующую информацию 
2.1. Из раздела ППР:
    наименование группы работы
    плановая сумма
    дата начала
    дата окончания
    факт сумма
    сведения о выполнении
    причина отклонения

2.2. Из раздела Работы по ТО:
    наименование группы работы
    плановая сумма
    дата начала
    дата окончания
    факт сумма
    сведения о выполнении
    причина отклонения

Примечание: если услуг несколько, но с разными поставщиками - тянем информацию из максимально заполненной услуги.
если заполнено одинаково - из любой.

Если по работе план уже существует, то дублирующая запись не создается";

        public override string Name => "Создание планов работ по содержанию и ремонту (2013)";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var repairServiceWithoutPlanQuery = this.RepairServiceRepository.GetAll()
                .Where(x => x.DisclosureInfoRealityObj.PeriodDi.DateStart.Value.Year == 2013)
                .Where(x => x.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedMo)
                .Where(
                    x => this.PlanWorkServiceRepairDomainService.GetAll()
                        .Where(y => y.DisclosureInfoRealityObj.Id == x.DisclosureInfoRealityObj.Id)
                        .Where(y => y.BaseService.TemplateService.Id == x.TemplateService.Id)
                        .Count() == 0);

            var planToCreate = repairServiceWithoutPlanQuery
                .Select(
                    x => new
                    {
                        BaseServiceId = x.Id,
                        x.TemplateService.Id,
                        DisclosureInfoRealityObjId = x.DisclosureInfoRealityObj.Id
                    })
                .AsEnumerable()
                .GroupBy(x => x.DisclosureInfoRealityObjId)
                .Select(
                    x => new
                    {
                        DisclosureInfoRealityObjId = x.Key,
                        serviceGroupList = x.GroupBy(y => y.Id).Select(y => y.Select(z => z.BaseServiceId).ToList()).ToList()
                    })
                .ToList();

            if (!planToCreate.Any())
            {
                return new BaseDataResult {Success = true};
            }

            var planWorks = this.WorkRepairListTatRepository.GetAll()
                .Where(x => repairServiceWithoutPlanQuery.Select(y => y.Id).Contains(x.BaseService.Id))
                .Select(
                    x => new
                    {
                        BaseServiceId = x.BaseService.Id,
                        x.Id,
                        x.PlannedCost,
                        x.FactCost,
                        x.DateStart,
                        x.DateEnd,
                        x.ReasonRejection,
                        x.InfoAboutExec
                    })
                .AsEnumerable()
                .GroupBy(x => x.BaseServiceId)
                .ToDictionary(x => x.Key, x => new {workList = x.ToList(), worksCount = x.Count()});

            var i = 0;
            const int InsertsPerTransaction = 100;

            while (i < planToCreate.Count)
            {
                var limit = (i + InsertsPerTransaction) < planToCreate.Count ? (i + InsertsPerTransaction) : planToCreate.Count;

                this.InTransaction(
                    () =>
                    {
                        for (; i < limit; ++i)
                        {
                            var plansByRealtyObject = planToCreate[i];

                            foreach (var serviceGroup in plansByRealtyObject.serviceGroupList)
                            {
                                // Получаем услугу, с максимальным количеством работ
                                var baseServiceId = serviceGroup.Count == 1
                                    ? serviceGroup.First()
                                    : serviceGroup.Select(
                                        x => new {BaseServiceId = x, worksCount = planWorks.ContainsKey(x) ? planWorks[x].worksCount : 0})
                                        .OrderByDescending(x => x.worksCount)
                                        .Select(x => x.BaseServiceId)
                                        .First();

                                var planWorkServiceRepair = new PlanWorkServiceRepair
                                {
                                    DisclosureInfoRealityObj = new DisclosureInfoRealityObj {Id = plansByRealtyObject.DisclosureInfoRealityObjId},
                                    BaseService = new BaseService {Id = baseServiceId}
                                };

                                this.PlanWorkServiceRepairDomainService.Save(planWorkServiceRepair);

                                if (!planWorks.ContainsKey(baseServiceId))
                                {
                                    continue;
                                }

                                foreach (var work in planWorks[baseServiceId].workList)
                                {
                                    var planWorkServiceRepairWork = new PlanWorkServiceRepairWorks
                                    {
                                        PlanWorkServiceRepair = planWorkServiceRepair,
                                        WorkRepairList = new WorkRepairListTat {Id = work.Id},
                                        Cost = work.PlannedCost,
                                        FactCost = work.FactCost,
                                        DateStart = work.DateStart,
                                        DateEnd = work.DateEnd,
                                        ReasonRejection = work.ReasonRejection,
                                        DataComplete = work.InfoAboutExec
                                    };

                                    this.PlanWorkServiceRepairWorksDomainService.Save(planWorkServiceRepairWork);
                                }
                            }
                        }
                    });
            }

            return new BaseDataResult {Success = true};
        }

        private void InTransaction(Action action)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    action();

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (TransactionRollbackException ex)
                    {
                        throw new DataAccessException(ex.Message, exc);
                    }
                    catch (Exception e)
                    {
                        throw new DataAccessException(
                            string.Format(
                                "Произошла неизвестная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
            }
        }
    }
}