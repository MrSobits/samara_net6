namespace Bars.Gkh.RegOperator.DomainModelServices.Impl.ChargeCalculators
{
    using System.Collections.Generic;
    using Entities.PersonalAccount;

    public class ChargeResult
    {
        public ChargeResult(TariffCharge tariffCharge, TariffRecalc tariffRecalc, PenaltyResult penalty)
        {
            Penalty = penalty;
            TariffRecalc = tariffRecalc;
            TariffCharge = tariffCharge;

            Traces = new List<CalculationParameterTrace>();
            Params = new List<PersonalAccountCalcParam_tmp>();
            RecalcHistory = new List<RecalcHistory>();
        }

        public TariffCharge TariffCharge { get; private set; }
        public TariffRecalc TariffRecalc { get; private set; }
        public PenaltyResult Penalty { get; private set; }

        public List<CalculationParameterTrace> Traces { get; private set; }
        public List<RecalcHistory> RecalcHistory { get; private set; }
        public List<PersonalAccountCalcParam_tmp> Params { get; private set; }
    }

    public class CalculationResult<T>
    {
        public CalculationResult()
        {
            Traces = new List<CalculationParameterTrace>();
            this.RecalcHistory = new List<RecalcHistory>();
        }

        public CalculationResult(T obj) : this()
        {
            Result = obj;
        }

        public T Result { get; private set; }

        public List<CalculationParameterTrace> Traces { get; private set; }
        
        /// <summary>
        /// Новая история расчетов
        /// </summary>
        public List<RecalcHistory> RecalcHistory{ get; private set; }
    }

    public struct TariffCharge
    {
        public TariffCharge(decimal byBaseTariff, decimal overplus) : this()
        {
            Overplus = overplus;
            ByBaseTariff = byBaseTariff;
        }

        public decimal ByBaseTariff { get; private set; }
        public decimal Overplus { get; private set; }
    }

    public struct TariffRecalc
    {
        public TariffRecalc(decimal byBaseTariff, decimal byDecisionTariff) : this()
        {
            ByDecisionTariff = byDecisionTariff;
            ByBaseTariff = byBaseTariff;
        }

        public decimal ByBaseTariff { get; private set; }
        public decimal ByDecisionTariff { get; private set; }
    }

    public struct PenaltyResult
    {
        public PenaltyResult(decimal penalty, decimal recalc) : this()
        {
            Recalc = recalc;
            Penalty = penalty;
        }

        public decimal Penalty { get; set; }
        public decimal Recalc { get; set; }
    }
}