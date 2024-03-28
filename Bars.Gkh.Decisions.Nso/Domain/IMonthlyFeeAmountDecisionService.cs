namespace Bars.Gkh.Decisions.Nso.Domain
{
    using Bars.B4;

    public interface IMonthlyFeeAmountDecisionService
    {
        IDataResult SaveHistory(BaseParams baseParams);
    }
}
