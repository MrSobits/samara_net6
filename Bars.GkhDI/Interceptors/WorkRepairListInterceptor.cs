namespace Bars.GkhDi.Interceptors
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhDi.Entities;

    public class WorkRepairListInterceptor : EmptyDomainInterceptor<WorkRepairList>
    {
        public override IDataResult AfterCreateAction(IDomainService<WorkRepairList> service, WorkRepairList entity)
        {
            var planWorkServ = Container.Resolve<IDomainService<PlanWorkServiceRepair>>();
            var worksServ = this.Container.Resolve<IDomainService<PlanWorkServiceRepairWorks>>();

            var planWorkAndService =
                Container.Resolve<IDomainService<PlanWorkServiceRepair>>().GetAll()
                    .FirstOrDefault(
                        x =>
                        x.DisclosureInfoRealityObj.Id == entity.BaseService.DisclosureInfoRealityObj.Id
                        && x.BaseService.Id == entity.BaseService.Id);

            if (planWorkAndService == null)
            {
                planWorkAndService = new PlanWorkServiceRepair
                {
                    DisclosureInfoRealityObj =
                        entity.BaseService.DisclosureInfoRealityObj,
                    BaseService = entity.BaseService
                };

                planWorkServ.Save(planWorkAndService);
            }

            worksServ.Save(
                new PlanWorkServiceRepairWorks
                {
                    PlanWorkServiceRepair = planWorkAndService,
                    WorkRepairList = entity,
                    Cost = entity.PlannedCost,
                    FactCost = entity.FactCost,
                    DateStart = entity.DateStart,
                    DateEnd = entity.DateEnd
                });

            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<WorkRepairList> service, WorkRepairList entity)
        {
            var planWorkServ = Container.Resolve<IDomainService<PlanWorkServiceRepair>>();
            var worksServ = this.Container.Resolve<IDomainService<PlanWorkServiceRepairWorks>>();

            var planWorkAndServiceWork =
                Container.Resolve<IDomainService<PlanWorkServiceRepairWorks>>().GetAll()
                    .FirstOrDefault(x => x.WorkRepairList.Id == entity.Id);

            if (planWorkAndServiceWork == null)
            {
                var planWorkAndService = Container.Resolve<IDomainService<PlanWorkServiceRepair>>().GetAll()
                                            .FirstOrDefault(x =>
                                                x.DisclosureInfoRealityObj.Id == entity.BaseService.DisclosureInfoRealityObj.Id && x.BaseService.Id == entity.BaseService.Id);

                if (planWorkAndService == null)
                {
                    planWorkAndService = new PlanWorkServiceRepair
                    {
                        DisclosureInfoRealityObj = entity.BaseService.DisclosureInfoRealityObj,
                        BaseService = entity.BaseService
                    };

                    planWorkServ.Save(planWorkAndService);
                }

                worksServ.Save(
                    new PlanWorkServiceRepairWorks
                    {
                        PlanWorkServiceRepair = planWorkAndService,
                        WorkRepairList = entity,
                        Cost = entity.PlannedCost,
                        FactCost = entity.FactCost,
                        DateStart = entity.DateStart,
                        DateEnd = entity.DateEnd
                    });
            }
            else
            {
                planWorkAndServiceWork.Cost = entity.PlannedCost;
                planWorkAndServiceWork.FactCost = entity.FactCost;
                planWorkAndServiceWork.DateStart = entity.DateStart;
                planWorkAndServiceWork.DateEnd = entity.DateEnd;

                worksServ.Update(planWorkAndServiceWork);
            }

            return this.Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<WorkRepairList> service, WorkRepairList entity)
        {
            var worksService = Container.Resolve<IDomainService<PlanWorkServiceRepairWorks>>();

            worksService.GetAll().Where(x => x.WorkRepairList.Id == entity.Id).Select(x => x.Id).ForEach(x => worksService.Delete(x));

            return Success();
        }
    }
}
