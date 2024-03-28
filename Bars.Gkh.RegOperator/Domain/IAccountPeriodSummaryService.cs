namespace Bars.Gkh.RegOperator.Domain
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Services.DataContracts.Accounts;

    public interface IAccountPeriodSummaryService
    {
        AccountPeriodSummary GetSummary(BasePersonalAccount account, ChargePeriod period, PersonalAccountPeriodSummary summary);
    }
}
