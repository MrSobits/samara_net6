namespace Bars.Gkh.RegOperator.DomainService
{
    using Bars.B4;

    public interface IDeliveryAgentService
    {
        IDataResult AddMunicipalities(BaseParams baseParams);

        IDataResult AddRealityObjects(BaseParams baseParams);

        IDataResult ListRealObjForDelAgent(BaseParams baseParams);

        IDataResult ListWithoutPaging(BaseParams baseParams);
    }
}