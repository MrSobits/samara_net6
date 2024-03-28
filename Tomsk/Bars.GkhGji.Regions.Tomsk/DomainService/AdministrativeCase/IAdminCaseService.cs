namespace Bars.GkhGji.Regions.Tomsk.DomainService
{
    using Bars.B4;

    public interface IAdminCaseService
    {
        IDataResult GetInfo(BaseParams baseParams);

        IDataResult ListView(BaseParams baseParams);
    }
}