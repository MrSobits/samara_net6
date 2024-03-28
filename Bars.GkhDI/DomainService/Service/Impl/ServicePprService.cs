namespace Bars.GkhDi.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using Castle.Windsor;
    using Entities;
    using Report;

    public class ServicePprService : IServicePprService
    {
        public IWindsorContainer Container { get; set; }

        public IEnumerable<RepairWorkDetailProxy> GetServicePpr(IQueryable<DisclosureInfoRealityObj> filterQuery)
        {
            return Container.ResolveDomain<WorkRepairList>().GetAll()
                .Where(x => filterQuery.Any(y => y.RealityObject.Id == x.BaseService.DisclosureInfoRealityObj.RealityObject.Id))
                .Select(x => new RepairWorkDetailProxy
                {
                    RealityObjectId = x.BaseService.DisclosureInfoRealityObj.RealityObject.Id,
                    BaseServiceId = x.BaseService.Id,
                    FactVolume = x.FactVolume,
                    PlanVolume = x.PlannedVolume,
                    Name = x.BaseService.TemplateService.Name,
                    UnitMeasure = x.BaseService.UnitMeasure.Name
                })
                .AsEnumerable();
        }
    }
}
