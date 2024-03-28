namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
{
    using Entities;
    using System.Collections;

    using B4;
    
    public interface IAppCitOperationsService
    {
        IDataResult CopyAppeal(BaseParams baseParams);

        IDataResult SyncEmailGJI(BaseParams baseParams);
    }
}