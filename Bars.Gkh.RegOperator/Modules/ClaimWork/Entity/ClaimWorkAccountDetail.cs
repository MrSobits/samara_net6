namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Modules.ClaimWork.Entities;

    /// <summary>
    /// Детализация ПИР по каждому лс
    /// </summary>
    public class ClaimWorkAccountDetail : BaseEntity
    {
        /// <summary>
        /// Лицевой счет
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Основание ПИР
        /// </summary>
        public virtual BaseClaimWork ClaimWork { get; set; }

        /// <summary>
        /// Сумма текущей задолженности по базовому тарифу
        /// </summary>
        public virtual decimal CurrChargeBaseTariffDebt { get; set; }

        /// <summary>
        /// Сумма текущей задолженности по тарифу решения
        /// </summary>
        public virtual decimal CurrChargeDecisionTariffDebt { get; set; }

        /// <summary>
        /// Сумма текущей задолженности
        /// </summary>
        public virtual decimal CurrChargeDebt { get; set; }

        /// <summary>
        /// Сумма текущей задолженности по пени
        /// </summary>
        public virtual decimal CurrPenaltyDebt { get; set; }

        /// <summary>
        /// Сумма исходной задолженности по базовому тарифу
        /// </summary>
        public virtual decimal OrigChargeBaseTariffDebt { get; set; }

        /// <summary>
        /// Сумма исходной задолженности по тарифу решения
        /// </summary>
        public virtual decimal OrigChargeDecisionTariffDebt { get; set; }

        /// <summary>
        /// Сумма исходной задолженности
        /// </summary>
        public virtual decimal OrigChargeDebt { get; set; }

        /// <summary>
        /// Сумма исходной задолженности по пени
        /// </summary>
        public virtual decimal OrigPenaltyDebt { get; set; }

        /// <summary>
        /// Дата начала отсчета
        /// </summary>
        public virtual DateTime StartingDate { get; set; }

        /// <summary>
        /// Количество дней просрочки
        /// </summary>
        public virtual int CountDaysDelay { get; set; }

        /// <summary>
        /// Число месяцев просрочки
        /// </summary>
        public virtual int CountMonthDelay { get; set; }
    }
}