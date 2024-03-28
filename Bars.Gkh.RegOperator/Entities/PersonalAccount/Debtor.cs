namespace Bars.Gkh.RegOperator.Entities.PersonalAccount
{
    using System;
    using Bars.B4.DataAccess;
    using Gkh.Enums.ClaimWork;
    using Gkh.Modules.ClaimWork.Entities;
    using Gkh.Enums;

    /// <summary>
    /// Неплательщик
    /// </summary>
    public class Debtor : BaseEntity
    {
        /// <summary>
        /// Лицевой счет
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Сумма задолженности
        /// </summary>
        public virtual decimal DebtSum { get; set; }

        /// <summary>
        /// Сумма задолженности по базовому тарифу
        /// </summary>
        public virtual decimal DebtBaseTariffSum { get; set; }

        /// <summary>
        /// Сумма задолженности по тарифу решения
        /// </summary>
        public virtual decimal DebtDecisionTariffSum { get; set; }

        /// <summary>
        /// Сумма задолженности по пени
        /// </summary>
        public virtual decimal PenaltyDebt { get; set; }

        /// <summary>
        /// Количество дней просрочки
        /// </summary>
        public virtual int ExpirationDaysCount { get; set; }

        /// <summary>
        /// Количество месяцев просрочки
        /// </summary>
        public virtual int ExpirationMonthCount { get; set; }

        /// <summary>
        /// Дата с которого идет отсчет времени
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// Тип судебного учреждения
        /// </summary>
        public virtual CourtType CourtType { get; set; }

        /// <summary>
        /// Судебное учреждение
        /// </summary>
        public virtual JurInstitution JurInstitution { get; set; }

        /// <summary>
        /// Имеется ли выписка из росреестра по лс
        /// </summary>
        public virtual YesNo? ExtractExists { get; set; }

        /// <summary>
        /// Дата выписки
        /// </summary>
        public virtual DateTime? ExtractDate { get; set; }

        /// <summary>
        /// Соответствует ли выписка данным на ЛС
        /// </summary>
        public virtual YesNo? AccountRosregMatched { get; set; }

        /// <summary>
        /// Обрабатывается ли ЛС агентом
        /// </summary>
        public virtual YesNo? ProcessedByTheAgent { get; set; }

        /// <summary>
        /// ПИР
        /// </summary>
        public virtual long? ClaimworkId { get; set; }

        /// <summary>
        /// Сумма задолженности по последнему ПИРу
        /// </summary>
        public virtual decimal LastClwDebt { get; set; }

        /// <summary>
        /// Сумма оплат после последнего пира
        /// </summary>
        public virtual decimal PaymentsSum { get; set; }

        /// <summary>
        /// Новая сумма задолженности
        /// </summary>
        public virtual decimal MewClaimDebt { get; set; }

        /// <summary>
        /// Новая сумма задолженности
        /// </summary>
        public virtual string LastPirPeriod { get; set; }
    }
}