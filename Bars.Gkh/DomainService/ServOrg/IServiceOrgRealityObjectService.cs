namespace Bars.Gkh.DomainService
{
    using Bars.B4;

    public interface IServiceOrgRealityObjectService
    {
        IDataResult AddRealityObjects(BaseParams baseParams);
        IDataResult GetInfo(BaseParams baseParams);
    }
}