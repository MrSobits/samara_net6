namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IProtocolViolationService
    {
        IDataResult ListRealityObject(BaseParams baseParams);

        IDataResult AddViolations(BaseParams baseParams);
    }
}