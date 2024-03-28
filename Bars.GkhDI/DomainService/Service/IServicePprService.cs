namespace Bars.GkhDi.DomainService
{
    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using Report;

    public interface IServicePprService
    {
        IEnumerable<RepairWorkDetailProxy> GetServicePpr(IQueryable<DisclosureInfoRealityObj> filterQuery);
    }
}
