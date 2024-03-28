namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;
    using Newtonsoft.Json;
    using PersonalAccount;
    using System.Collections.Generic;

    /// <summary>
    /// Ситуация по ЛС на период
    /// </summary>
    public partial class PersonalAccountPeriodSummary : BaseImportableEntity
    {
        private readonly IList<PeriodSummaryBalanceChange> balanceChanges;
        private readonly IList<PenaltyChange> penaltyChanges;

        public PersonalAccountPeriodSummary(BasePersonalAccount account, ChargePeriod period, decimal saldoIn) : this()
        {
            PersonalAccount = account;
            Period = period;
            SaldoIn = saldoIn;
        }

        public PersonalAccountPeriodSummary()
        {
            this.balanceChanges = new List<PeriodSummaryBalanceChange>();
            this.penaltyChanges = new List<PenaltyChange>();
        }

        /// <summary>
        /// ЛС
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Период начисления
        /// </summary>
        public virtual ChargePeriod Period { get; set; }

        /// <summary>
        /// Начислено по тарифу
        /// </summary>
        public virtual decimal ChargeTariff { get; set; }

        /// <summary>
        /// Начислено по базовому тарифу 
        /// (Которое принято на МО. Есть еще начисленное сверх базового тарифа, в случае, если решение собственников больше, чем базовый)
        /// </summary>
        public virtual decimal ChargedByBaseTariff { get; set; }

        /// <summary>
        /// Перерасчет за период
        /// </summary>
        public virtual decimal RecalcByBaseTariff { get; set; }

        /// <summary>
        /// Перерасчет за тариф решения
        /// </summary>
        public virtual decimal RecalcByDecisionTariff { get; set; }

        /// <summary>
        /// Перерасчет пени
        /// </summary>
        public virtual decimal RecalcByPenalty { get; set; }

        /// <summary>
        /// Пени
        /// </summary>
        public virtual decimal Penalty { get; set; }

        /// <summary>
        /// Оплачено пени
        /// </summary>
        public virtual decimal PenaltyPayment { get; set; }

        /// <summary>
        /// Оплачено по тарифу в текущем периоде(Оплачено по базовому тарифу)
        /// </summary>
        public virtual decimal TariffPayment { get; set; }

        /// <summary>
        /// Входящее сальдо
        /// </summary>
        public virtual decimal SaldoIn { get; set; }

        /// <summary>
        /// Исходящее сальдо
        /// </summary>
        public virtual decimal SaldoOut { get; set; }

        /// <summary>
        /// Оплачено по тарифу решения
        /// </summary>
        public virtual decimal TariffDecisionPayment { get; set; }

        /// <summary>
        /// Оплачено по типу услуги "Капремонт" (грузится импортом ЛС для раздела "Перечисления средств в фонд")
        /// </summary>
        public virtual decimal OverhaulPayment { get; set; }

        /// <summary>
        /// Оплачено по типу услуги "Найм" (грузится импортом ЛС для раздела "Перечисления средств в фонд")
        /// </summary>
        public virtual decimal RecruitmentPayment { get; set; }

        /// <summary>
        /// Входящее сальдо
        /// </summary>
        public virtual decimal? SaldoInFromServ { get; set; }

        /// <summary>
        /// Исходящее сальдо
        /// </summary>
        public virtual decimal? SaldoOutFromServ { get; set; }

        /// <summary>
        /// Изменение сальдо
        /// </summary>
        public virtual decimal? SaldoChangeFromServ { get; set; }

        /// <summary>
        /// Сумма операций установки/изменения сальдо по базовому тарифу за период
        /// </summary>
        public virtual decimal BaseTariffChange { get; set; }

        /// <summary>
        /// Сумма операций установки/изменения сальдо по тарифу решения за период
        /// </summary>
        public virtual decimal DecisionTariffChange { get; set; }

        /// <summary>
        /// Сумма операций установки/изменения сальдо по пени за период
        /// </summary>
        public virtual decimal PenaltyChange { get; set; }

        /// <summary>
        /// Задолженность по базовому тарифу на начало периода
        /// </summary>
        public virtual decimal BaseTariffDebt { get; set; }

        /// <summary>
        /// Задолженность по тарифу решения на начало периода
        /// </summary>
        public virtual decimal DecisionTariffDebt { get; set; }

        /// <summary>
        /// Задолженность по пени на начало периода
        /// </summary>
        public virtual decimal PenaltyDebt { get; set; }

        /// <summary>
        /// Зачет средств за работы по базовому тарифу
        /// </summary>
        public virtual decimal PerformedWorkChargedBase { get; set; }

        /// <summary>
        /// Зачет средств за работы по тарифу решения
        /// </summary>
        public virtual decimal PerformedWorkChargedDecision { get; set; }

        /// <summary>
        /// Операции изменения баланса за период
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<PeriodSummaryBalanceChange> BalanceChanges
        {
            get { return this.balanceChanges; }
        }

        /// <summary>
        /// Коллекция ручных изменений пеней
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<PenaltyChange> PenaltyChanges
        {
            get { return this.penaltyChanges; }
        }
    }
}