namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;

    using Gkh.Modules.ClaimWork.Entities;

    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// Эталонный расчет даты начала задолженности
    /// </summary>
    public class DebtorReferenceCalculation : BaseEntity
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
        public virtual Int64 PeriodId { get; set; }

        /// <summary>
        /// Лицевой счет
        /// </summary>
        public virtual Int64 PersonalAccountId { get; set; }


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
        public virtual AgentPIRDebtor AgentPIRDebtor { get; set; }

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
        public virtual decimal? Penalties { get; set; }

        /// <summary>
        /// Оплата Пени
        /// </summary>
        public virtual decimal? PenaltyPayment { get; set; }

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