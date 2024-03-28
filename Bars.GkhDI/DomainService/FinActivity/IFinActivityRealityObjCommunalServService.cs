namespace Bars.GkhDi.DomainService
{
    using Bars.B4;

    public interface IFinActivityRealityObjCommunalServService
    {
        IDataResult AddWorkMode(BaseParams baseParams);

        IDataResult AddDataByRealityObj(BaseParams baseParams);
    }
}