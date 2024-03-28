namespace Bars.Gkh.RegOperator.DomainService
{
    using Bars.B4;

    public interface ICashPaymentCenterService
    {
        IDataResult AddMunicipalities(BaseParams baseParams);

        IDataResult AddObjects(BaseParams baseParams);

        IDataResult DeleteObjects(BaseParams baseParams);

        IDataResult ListObjForCashPaymentCenter(BaseParams baseParams);

        IDataResult ListRealObjForCashPaymentCenterManOrg(BaseParams baseParams);

        IDataResult ListWithoutPaging(BaseParams baseParams);

        IDataResult SetCashPaymentCenters(BaseParams baseParams);
    }
}