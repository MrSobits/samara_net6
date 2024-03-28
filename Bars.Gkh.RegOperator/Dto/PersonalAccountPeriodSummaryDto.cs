namespace Bars.Gkh.RegOperator.Dto
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Ситуация по ЛС на период
    /// </summary>
    public class PersonalAccountPeriodSummaryDto
    {
        /// <summary>
        /// ЛС
        /// </summary>
        public long PersonalAccountId { get; set; }

        /// <summary>
        /// Период начисления
        /// </summary>
        public ChargePeriod Period { get; set; }

        /// <summary>
        /// Период начисления
        /// </summary>
        public long PeriodId { get; set; }

        /// <summary>
        /// Начислено по тарифу (базовый + решения)
        /// </summary>
        public decimal ChargeTariff { get; set; }

        /// <summary>
        /// Начислено по базовому тарифу 
        /// (Которое принято на МО. Есть еще начисленное сверх базового тарифа, в случае, если решение собственников больше, чем базовый)
        /// </summary>
        public decimal ChargedByBaseTariff { get; set; }

        /// <summary>
        /// Перерасчет за период
        /// </summary>
        public decimal RecalcByBaseTariff { get; set; }

        /// <summary>
        /// Перерасчет за тариф решения
        /// </summary>
        public decimal RecalcByDecisionTariff { get; set; }

        /// <summary>
        /// Перерасчет пени
        /// </summary>
        public decimal RecalcByPenalty { get; set; }

        /// <summary>
        /// Пени
        /// </summary>
        public decimal Penalty { get; set; }

        /// <summary>
        /// Оплачено пени
        /// </summary>
        public decimal PenaltyPayment { get; set; }

        /// <summary>
        /// Оплачено по тарифу в текущем периоде(Оплачено по базовому тарифу)
        /// </summary>
        public decimal TariffPayment { get; set; }

        /// <summary>
        /// Входящее сальдо
        /// </summary>
        public decimal SaldoIn { get; set; }

        /// <summary>
        /// Исходящее сальдо
        /// </summary>
        public decimal SaldoOut { get; set; }

        /// <summary>
        /// Оплачено по тарифу решения
        /// </summary>
        public decimal TariffDecisionPayment { get; set; }

        /// <summary>
        /// Оплачено по типу услуги "Капремонт" (грузится импортом ЛС для раздела "Перечисления средств в фонд")
        /// </summary>
        public decimal OverhaulPayment { get; set; }

        /// <summary>
        /// Оплачено по типу услуги "Найм" (грузится импортом ЛС для раздела "Перечисления средств в фонд")
        /// </summary>
        public decimal RecruitmentPayment { get; set; }

        /// <summary>
        /// Входящее сальдо
        /// </summary>
        public decimal? SaldoInFromServ { get; set; }

        /// <summary>
        /// Исходящее сальдо
        /// </summary>
        public decimal? SaldoOutFromServ { get; set; }

        /// <summary>
        /// Изменение сальдо
        /// </summary>
        public decimal? SaldoChangeFromServ { get; set; }

        /// <summary>
        /// Сумма операций установки/изменения сальдо по базовому тарифу за период
        /// </summary>
        public decimal BaseTariffChange { get; set; }

        /// <summary>
        /// Сумма операций установки/изменения сальдо по тарифу решения за период
        /// </summary>
        public decimal DecisionTariffChange { get; set; }

        /// <summary>
        /// Сумма операций установки/изменения сальдо по пени за период
        /// </summary>
        public decimal PenaltyChange { get; set; }

        /// <summary>
        /// Задолженность по базовому тарифу на начало периода
        /// </summary>
        public decimal BaseTariffDebt { get; set; }

        /// <summary>
        /// Задолженность по тарифу решения на начало периода
        /// </summary>
        public decimal DecisionTariffDebt { get; set; }

        /// <summary>
        /// Задолженность по пени на начало периода
        /// </summary>
        public decimal PenaltyDebt { get; set; }

        /// <summary>
        /// Зачет средств за работы
        /// </summary>
        public decimal PerformedWorkCharged { get; set; }

        /// <summary>
        /// Зачет средств за работы по тарифу решения
        /// </summary>
        public decimal DecsicionPerformedWorkCharged { get; set; }

        public void ApplyCharge(decimal baseTariff, decimal decisionTariff, decimal penalty, decimal recalcByPenalty, decimal recalcByTariff, decimal recalByDecision)
        {
            this.ChargedByBaseTariff += baseTariff;
            this.ChargeTariff += decisionTariff + baseTariff;
            this.Penalty += penalty;
            this.RecalcByBaseTariff += recalcByTariff;
            this.RecalcByDecisionTariff += recalByDecision;
            this.RecalcByPenalty += recalcByPenalty;

            this.RecalcSaldoOut();
        }

        public void ApplyCharge(PersonalAccountChargeDto charge)
        {
            this.ApplyCharge(charge.ChargeTariff - charge.OverPlus, charge.OverPlus, charge.Penalty, charge.RecalcPenalty, charge.RecalcByBaseTariff, charge.RecalcByDecisionTariff);
        }

        public void UndoCharge(PersonalAccountCharge charge)
        {
            this.ChargedByBaseTariff -= charge.ChargeTariff - charge.OverPlus;
            this.ChargeTariff -= charge.ChargeTariff;
            this.Penalty -= charge.Penalty;
            this.RecalcByBaseTariff -= charge.RecalcByBaseTariff;
            this.RecalcByDecisionTariff -= charge.RecalcByDecisionTariff;
            this.RecalcByPenalty -= charge.RecalcPenalty;

            this.RecalcSaldoOut();
        }

        /// <summary>
        /// Пересчитать исходящее сальдо
        /// </summary>
        /// <returns></returns>
        public void RecalcSaldoOut()
        {
            this.SaldoOut = (this.SaldoIn + this.GetTotalCharge() + this.GetTotalChange() - this.GetTotalPayment()).RegopRoundDecimal(2);
        }

        /// <summary>
        /// Подсчет суммы по дебету за период
        /// </summary>
        /// <returns></returns>
        public decimal GetTotalCharge()
        {
            return this.ChargeTariff + this.Penalty + this.RecalcByBaseTariff + this.RecalcByDecisionTariff + this.RecalcByPenalty - this.PerformedWorkCharged - this.DecsicionPerformedWorkCharged;
        }

        /// <summary>
        /// Подсчёт суммы изменений сальдо по периоду
        /// </summary>
        /// <returns></returns>
        public decimal GetTotalChange()
        {
            return this.BaseTariffChange + this.DecisionTariffChange + this.PenaltyChange;
        }

        /// <summary>
        /// Подсчет суммы по кредиту за период
        /// </summary>
        /// <returns></returns>
        public decimal GetTotalPayment()
        {
            return this.TariffPayment + this.TariffDecisionPayment + this.PenaltyPayment;
        }
    }
}