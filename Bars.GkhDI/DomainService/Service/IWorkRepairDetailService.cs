namespace Bars.GkhDi.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Report.Fucking731;

    public interface IWorkRepairDetailService
    {
        IDataResult AddWorks(BaseParams baseParams);

        Dictionary<long, Dictionary<long, DisclosureInfo731.ObjectRepairServiceDetail[]>> GetRepairDetailsDict(IQueryable<long> diroIdQuery);
    }
}