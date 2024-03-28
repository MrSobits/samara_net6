namespace Bars.GkhCr.DomainService
{
    using B4;

    public interface ISpecialMonitoringSmrService
    {
        IDataResult GetByObjectCrId(BaseParams baseParams);

        IDataResult SaveByObjectCrId(BaseParams baseParams);
    }
}
