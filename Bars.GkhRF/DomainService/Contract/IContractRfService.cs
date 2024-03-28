namespace Bars.GkhRf.DomainService
{
    using Bars.B4;

    public interface IContractRfService
    {
        IDataResult ActualRealityObjectList(BaseParams baseParams);

        IDataResult ListByManOrg(BaseParams baseParams);

        IDataResult CheckAvailableRealObj(BaseParams baseParams);

        /// <summary>
        /// Возвращает жилые дома у которых заключен договор с УО  позже заданной даты 
        /// </summary>
        /// <returns></returns>
        IDataResult ListByManOrgAndContractDate(BaseParams baseParams);
    }
}