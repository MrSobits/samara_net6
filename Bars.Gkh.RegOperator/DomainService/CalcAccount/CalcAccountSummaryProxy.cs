namespace Bars.Gkh.RegOperator.DomainService
{
    public class CalcAccountSummaryProxy
    {
        public long AccountId { get; set; }

        public decimal Credit { get; set; }

        public decimal Debt { get; set; }

        public decimal LoanSum { get; set; }

        public decimal MoneyLocks { get; set; }

        public decimal LoanReturnedSum { get; set; }

        public decimal Saldo => this.Debt - this.Credit;

        public decimal PercentSum { get; set; }
    }
}
