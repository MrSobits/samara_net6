namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IActivityTsjRealObjService
    {
        IDataResult AddRealityObjects(BaseParams baseParams);

        IDataResult GetInfo(long? activityTsjIdId);
    }
}