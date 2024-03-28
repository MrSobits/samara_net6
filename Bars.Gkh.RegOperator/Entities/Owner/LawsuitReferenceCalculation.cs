namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Gkh.Modules.ClaimWork.Entities;

            
    /// <summary>
    /// Эталонный расчет даты начала задолженности
    /// </summary>
    public class LawsuitReferenceCalculation : BaseEntity
    {
        /// <summary>
        /// Номер лицевого счета
        /// </summary>
        public virtual string AccountNumber { get; set; }

        /// <summary>
        /// Дата оплаты
        /// </summary>
        public virtual string PaymentDate { get; set; }

        /// <summary>
        /// Период начисления
        /// </summary>
        public virtual ChargePeriod PeriodId { get; set; }

        /// <summary>
        /// Лицевой счет
        /// </summary>
        public virtual BasePersonalAccount PersonalAccountId { get; set; }

        /// <summary>
        /// Тариф
        /// </summary>
        public virtual decimal BaseTariff { get; set; }

        /// <summary>
        /// Площадь лицевого счета
        /// </summary>
        public virtual decimal RoomArea { get; set; }

        /// <summary>
        /// Доля лицевого счета
        /// </summary>
        public virtual decimal AreaShare { get; set; }

        /// <summary>
        /// Исковое зявление
        /// </summary>
        public virtual Lawsuit Lawsuit { get; set; }

        /// <summary>
        /// Начислено
        /// </summary>
        public virtual decimal TariffCharged { get; set; }

        /// <summary>
        /// Уплачено
        /// </summary>
        public virtual decimal TarifPayment { get; set; }

        /// <summary>
        /// Задолженность
        /// </summary>
        public virtual decimal TarifDebt { get; set; }

        /// <summary>
        /// Задолженность с учетом погашений
        /// </summary>
        public virtual decimal TarifDebtPay { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// Пени
        /// </summary>
        public virtual decimal Penalties { get; set; }

        /// <summary>
        /// Оплата Пени
        /// </summary>
        public virtual decimal PenaltyPayment { get; set; }

        /// <summary>
        /// Дата оплаты пени
        /// </summary>
        public virtual string PenaltyPaymentDate { get; set; }

        /// <summary>
        /// Протокол начисления пени
        /// </summary>
        public virtual string AccrualPenalties { get; set; }

        /// <summary>
        /// Протокол начисления пени
        /// </summary>
        public virtual string AccrualPenaltiesFormula { get; set; }
        
    }
}