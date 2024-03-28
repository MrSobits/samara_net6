namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using Bars.Gkh.Entities;

    using Domain.ProxyEntity;
    using Entities;

    public interface IRecalcCalculator
    {
        CalculationResult<Recalc> Calculate(BasePersonalAccount account, IPeriod period, UnacceptedCharge unAccepted);
    }

    public class Recalc
    {
        public decimal ByBaseTariff { get; set; }

        public decimal ByDecisionTariff { get; set; }
    }
}