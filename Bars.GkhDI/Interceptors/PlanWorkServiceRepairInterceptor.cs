namespace Bars.GkhDi.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Entities;

    public class PlanWorkServiceRepairInterceptor : EmptyDomainInterceptor<PlanWorkServiceRepair>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<PlanWorkServiceRepair> service, PlanWorkServiceRepair entity)
        {
            var planWorkServiceRepairWorksService = this.Container.Resolve<IDomainService<PlanWorkServiceRepairWorks>>();
            var planWorkServiceRepairWorksIds = planWorkServiceRepairWorksService.GetAll().Where(x => x.PlanWorkServiceRepair.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in planWorkServiceRepairWorksIds)
            {
                planWorkServiceRepairWorksService.Delete(id);
            }

            return Success();
        }
    }
}
