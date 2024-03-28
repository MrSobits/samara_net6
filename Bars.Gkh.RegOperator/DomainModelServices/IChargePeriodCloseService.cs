namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using B4;

    public interface IChargePeriodCloseService
    {
        IDataResult CloseCurrentPeriod(BaseParams baseParams);
    }
}
