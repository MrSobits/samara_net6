using System.Linq;

namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    using B4;
    using Bars.GkhGji.DomainService.Contracts;
    using Entities;

    public interface IPreventiveVisitService
    { 
        IDataResult GetInfo(BaseParams baseParams);

        IDataResult GetListRoForResultPV(BaseParams baseParams);

        IDataResult AddRealityObjects(BaseParams baseParams);

    }
}