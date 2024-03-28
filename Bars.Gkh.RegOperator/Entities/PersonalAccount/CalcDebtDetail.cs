namespace Bars.Gkh.RegOperator.Entities.PersonalAccount
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Детализация по операции расчета долга
    /// </summary>
    public class CalcDebtDetail : BaseEntity
    {
        /// <summary>
        /// Операция расчета долга
        /// </summary>
        public virtual PersonalAccountCalcDebt CalcDebt { get; set; }

        /// <summary>
        /// Абонент
        /// </summary>
        public virtual PersonalAccountOwner AccountOwner { get; set; }

        /// <summary>
        /// Тип
        /// </summary>
        public virtual DebtDetailType Type { get; set; }

        /// <summary>
        /// Начислено по БТ
        /// </summary>
        public virtual decimal ChargeBaseTariff { get; set; }

        /// <summary>
        /// Распределение долга по БТ
        /// </summary>
        public virtual decimal DistributionDebtBaseTariff { get; set; }

        /// <summary>
        /// Начислено по ТР
        /// </summary>
        public virtual decimal ChargeDecTariff { get; set; }

        /// <summary>
        /// Распределение долга по ТР
        /// </summary>
        public virtual decimal DistributionDebtDecTariff { get; set; }

        /// <summary>
        /// Начислено по пени
        /// </summary>
        public virtual decimal ChargePenalty { get; set; }

        /// <summary>
        /// Распределение долга по пени
        /// </summary>
        public virtual decimal DistributionDebtPenalty { get; set; }

        /// <summary>
        /// Оплата по БТ
        /// </summary>
        public virtual decimal PaymentBaseTariff { get; set; }

        /// <summary>
        /// Распределение оплаты по БТ
        /// </summary>
        public virtual decimal DistributionPayBaseTariff { get; set; }

        /// <summary>
        /// Оплата по ТР
        /// </summary>
        public virtual decimal PaymentDecTariff { get; set; }

        /// <summary>
        /// Распределение оплаты по ТР
        /// </summary>
        public virtual decimal DistributionPayDecTariff { get; set; }

        /// <summary>
        /// Оплата по пени
        /// </summary>
        public virtual decimal PaymentPenalty { get; set; }

        /// <summary>
        /// Распределение оплаты по пени
        /// </summary>
        public virtual decimal DistributionPayPenalty { get; set; }

        /// <summary>
        /// Исходящее сальдо по БТ
        /// </summary>
        public virtual decimal SaldoOutBaseTariff { get; set; }

        /// <summary>
        /// Исходящее сальдо по ТР
        /// </summary>
        public virtual decimal SaldoOutDecisionTariff { get; set; }

        /// <summary>
        /// 	Исходящее сальдо по пени
        /// </summary>
        public virtual decimal SaldoOutPenalty { get; set; }

        /// <summary>
        /// Прежний ЛС
        /// </summary>
        public virtual BasePersonalAccount PrevPersonalAccount { get; set; }

        /// <summary>
        /// ЛС импортирвоан
        /// </summary>
        public virtual bool IsImported { get; set; }
    }
}