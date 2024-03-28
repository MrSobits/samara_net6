using Bars.B4;

namespace Bars.Gkh.Repair.DomainService
{
    using System.Linq;

    using Entities;

    public interface IRepairObjectService
    {
        IQueryable<RepairObject> GetFilteredByOperator();
        IDataResult MassStateChange(BaseParams baseParams);
    }
}