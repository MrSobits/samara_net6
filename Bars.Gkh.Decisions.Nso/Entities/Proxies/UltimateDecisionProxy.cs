namespace Bars.Gkh.Decisions.Nso.Entities.Proxies
{
    using Decisions;

    public class UltimateDecisionProxy
    {
        public RealityObjectDecisionProtocol Protocol { get; set; }

        public AccountOwnerDecision AccountOwnerDecision { get; set; }

        public AccumulationTransferDecision AccumulationTransferDecision { get; set; }

        public CreditOrgDecision CreditOrgDecision { get; set; }

        public CrFundFormationDecision CrFundFormationDecision { get; set; }

        public MinFundAmountDecision MinFundAmountDecision { get; set; }

        public MkdManagementDecision MkdManagementDecision { get; set; }

        public MonthlyFeeAmountDecision MonthlyFeeAmountDecision { get; set; }

        public JobYearDecision JobYearDecision { get; set; }

        public AccountManagementDecision AccountManagementDecision { get; set; }

        public PenaltyDelayDecision PenaltyDelayDecision { get; set; }

        public PaymentAndFundDecisions PaymentAndFundDecisions { get; set; }
    }
}