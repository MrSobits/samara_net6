namespace Bars.GkhCr.DomainService
{
    using B4;

    public interface IMonitoringSmrService
    {
        IDataResult GetByObjectCrId(BaseParams baseParams);

        IDataResult SaveByObjectCrId(BaseParams baseParams);
    }
}
