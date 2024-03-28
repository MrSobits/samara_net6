namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    using Entities;
    using System.Collections;

    using B4;
    
    public interface IProtocolOSPRequestOperationsService
    {
        IDataResult SendAnswer(BaseParams baseParams);

        IDataResult GetDocInfo(BaseParams baseParams);
    }
}