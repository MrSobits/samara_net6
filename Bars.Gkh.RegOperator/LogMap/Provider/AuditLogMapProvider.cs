namespace Bars.Gkh.RegOperator.LogMap
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Decisions.Nso.LogMap;

    public class AuditLogMapProvider : IAuditLogMapProvider
    {
        public void Init(IAuditLogMapContainer container)
        {
            container.Add<PersonalAccountOwnerLogMap>();
            container.Add<BasePersonalAccountLogMap>();
            container.Add<PersonalAccountPeriodSummaryLogMap>();
            container.Add<RegOperatorLogMap>();
            container.Add<FundFormationContractLogMap>();
            container.Add<RealityObjectDecisionProtocolLogMap>();
            container.Add<GovDecisionLogMap>();
            container.Add<AccountManagementDecisionLogMap>();
            container.Add<AccountOwnerDecisionLogMap>();
            container.Add<AccumulationTransferDecisionLogMap>();
            container.Add<CreditOrgDecisionLogMap>();
            container.Add<CrFundFormationDecisionLogMap>();
            container.Add<MinFundAmountDecisionLogMap>();
            container.Add<MkdManagementDecisionLogMap>();
            container.Add<PenaltyDelayDecisionLogMap>();
            container.Add<ChargePeriodLogMap>();
        }
    }
}