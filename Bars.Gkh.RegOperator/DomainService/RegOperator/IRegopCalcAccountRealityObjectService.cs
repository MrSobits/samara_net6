namespace Bars.Gkh.RegOperator.DomainService
{
    using B4;

    public interface IRegopCalcAccountRealityObjectService
    {
        IDataResult MassCreate(BaseParams baseParams);

        IDataResult ListAccounts(BaseParams baseParams);

        IDataResult ListSpecialAccounts(BaseParams baseParams);

        IDataResult ListOperations(BaseParams baseParams);

        IDataResult GetRegopMoneyInfo(BaseParams baseParams);

        IDataResult ListRealObjForRegopCalcAcc(BaseParams baseParams);
    }
}