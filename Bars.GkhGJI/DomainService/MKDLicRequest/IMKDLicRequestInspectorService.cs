namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    using System.Linq;
    using Entities;

    public interface IMKDLicRequestInspectorService
    {
        IDataResult AddInspectors(BaseParams baseParams);

        IDataResult GetInfo(BaseParams baseParams);
    }
}