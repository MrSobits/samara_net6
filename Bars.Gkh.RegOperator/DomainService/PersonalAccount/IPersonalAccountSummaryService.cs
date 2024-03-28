namespace Bars.Gkh.RegOperator.DomainService
{
    using Bars.B4;

    public interface IPersonalAccountSummaryService
    {
        IDataResult GetPeriodAccountSummary(BaseParams baseParams);

        IDataResult ListChargeParameterTrace(BaseParams baseParams);

        IDataResult ListPenaltyParameterTrace(BaseParams baseParams);

        IDataResult ListReCalcParameterTrace(BaseParams baseParams);

        IDataResult ListRecalcPenaltyTrace(BaseParams baseParams);

        IDataResult GetAccountSummaryInfoInCurrentPeriod(BaseParams baseParams);

        IDataResult ListRecalcPenaltyTraceDetail(BaseParams baseParams);
    }
}