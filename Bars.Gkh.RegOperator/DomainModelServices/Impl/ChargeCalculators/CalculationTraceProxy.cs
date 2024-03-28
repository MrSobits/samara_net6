namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.ChargeCalculators
{
    using System;

    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;

    internal class CalculationTraceProxy
    {
        public decimal Tariff { get; set; }

        public decimal DecisionTariff { get; set; }

        public decimal Share { get; set; }

        public decimal Area { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public TariffSource TariffSource { get; set; }

        public DateTime DateActualArea { get; set; }

        public DateTime DateActualShare { get; set; }
    }
}