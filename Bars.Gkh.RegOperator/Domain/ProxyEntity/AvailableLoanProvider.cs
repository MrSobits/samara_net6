namespace Bars.Gkh.RegOperator.Domain.ProxyEntity
{
    using Enums;
    using Gkh.Entities;

    public class AvailableLoanProvider
    {
        public string Address { get; set; }

        public RealityObject RealityObject { get; set; }

        public int PlanYear { get; set; }

        public decimal AvailableMoney { get; set; }

        public TypeSourceLoan TypeSourceLoan { get; set; }
    }
}