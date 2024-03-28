namespace Bars.GkhDi.Regions.Tatarstan.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Regions.Tatarstan.Entities;

    public class WorkRepairListTatInterceptor : EmptyDomainInterceptor<WorkRepairListTat>
    {
        public override IDataResult AfterCreateAction(IDomainService<WorkRepairListTat> service, WorkRepairListTat entity)
        {
            var planWorkServ = Container.Resolve<IDomainService<PlanWorkServiceRepair>>();
            var worksServ = Container.Resolve<IDomainService<PlanWorkServiceRepairWorks>>();

            var planWorkAndService = planWorkServ.GetAll()
                    .FirstOrDefault(x => x.DisclosureInfoRealityObj.Id == entity.BaseService.DisclosureInfoRealityObj.Id
                        && x.BaseService.Id == entity.BaseService.Id);

            if (planWorkAndService == null)
            {
                planWorkAndService = new PlanWorkServiceRepair
                {
                    DisclosureInfoRealityObj = entity.BaseService.DisclosureInfoRealityObj,
                    BaseService = entity.BaseService
                };

                planWorkServ.Save(planWorkAndService);
            }

            worksServ.Save(new PlanWorkServiceRepairWorks
            {
                PlanWorkServiceRepair = planWorkAndService,
                WorkRepairList = entity,
                Cost = entity.PlannedCost,
                FactCost = entity.FactCost,
                DateStart = entity.DateStart,
                DateEnd = entity.DateEnd,
                ReasonRejection = entity.ReasonRejection,
                DataComplete = entity.InfoAboutExec
            });

            return Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<WorkRepairListTat> service, WorkRepairListTat entity)
        {
            var planWorkServ = Container.Resolve<IDomainService<PlanWorkServiceRepair>>();
            var worksServ = Container.Resolve<IDomainService<PlanWorkServiceRepairWorks>>();

            var planWorkAndServiceWork = worksServ.GetAll()
                .FirstOrDefault(x => x.WorkRepairList.Id == entity.Id);

            if (planWorkAndServiceWork == null)
            {
                var planWorkAndService = planWorkServ.GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.Id == entity.BaseService.DisclosureInfoRealityObj.Id)
                    .FirstOrDefault(x => x.BaseService.Id == entity.BaseService.Id);

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
                            DateEnd = entity.DateEnd,
                            ReasonRejection = entity.ReasonRejection,
                            DataComplete = entity.InfoAboutExec
                        });
            }
            else
            {
                planWorkAndServiceWork.Cost = entity.PlannedCost;
                planWorkAndServiceWork.FactCost = entity.FactCost;
                planWorkAndServiceWork.DateStart = entity.DateStart;
                planWorkAndServiceWork.DateEnd = entity.DateEnd;
                planWorkAndServiceWork.ReasonRejection = entity.ReasonRejection;
                planWorkAndServiceWork.DataComplete = entity.InfoAboutExec;

                worksServ.Update(planWorkAndServiceWork);
            }

            return this.Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<WorkRepairListTat> service, WorkRepairListTat entity)
        {
            var worksService = Container.Resolve<IDomainService<PlanWorkServiceRepairWorks>>();

            worksService.GetAll().Where(x => x.WorkRepairList.Id == entity.Id).Select(x => x.Id).ForEach(x => worksService.Delete(x));

            return Success();
        }
    }
}
