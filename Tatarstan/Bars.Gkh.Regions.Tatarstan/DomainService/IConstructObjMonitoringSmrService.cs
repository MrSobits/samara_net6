namespace Bars.Gkh.Regions.Tatarstan.DomainService
{
    using Bars.B4;

    public interface IConstructObjMonitoringSmrService
    {
        IDataResult GetByConstructObjectId(BaseParams baseParams);

        IDataResult SaveByConstructObjectId(BaseParams baseParams);
    }
}
