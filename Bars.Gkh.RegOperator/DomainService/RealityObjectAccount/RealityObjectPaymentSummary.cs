namespace Bars.Gkh.RegOperator.DomainService
{
    public class RealityObjectPaymentSummary
    {
        public long RealityObjectId { get; set; }

        public decimal Credit { get; set; }

        public decimal Debt { get; set; }

        public decimal PercentSum { get; set; }

        public decimal Saldo
        {
            get
            {
                return Debt - Credit;
            }
        }
    }
}