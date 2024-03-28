namespace Bars.Gkh.RegOperator.Modules.ClaimWork.Entity
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Представление "Должник"
    /// </summary>
    /*
     * Вьюха должников, для экспорта и вьюмодели
     */
    public class ViewDebtorExport : PersistentObject
    {
        /// <summary>
        /// Id аккаунта
        /// </summary>
        public virtual long? PersonalAccountId { get; set; }

        /// <summary>
        /// Id муниципального образования
        /// </summary>
        public virtual long MunicipalityId { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual string Municipality { get; set; }

        /// <summary>
        /// МО - поселение
        /// </summary>
        public virtual string Settlement { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string RoomAddress { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual long StateId { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual string State { get; set; }

        /// <summary>
        /// Номер ЛС
        /// </summary>
        public virtual string PersonalAccountNum { get; set; }

        /// <summary>
        /// Id собственника
        /// </summary>
        public virtual long AccountOwnerId { get; set; }

        /// <summary>
        /// Собственник
        /// </summary>
        public virtual string AccountOwner { get; set; }

        /// <summary>
        /// Тип собственника
        /// </summary>
        public virtual PersonalAccountOwnerType OwnerType { get; set; }

        /// <summary>
        /// Сумма долга
        /// </summary>
        public virtual decimal DebtSum { get; set; }

        /// <summary>
        /// Сумма по базовому тарифу
        /// </summary>
        public virtual decimal DebtBaseTariffSum { get; set; }

        /// <summary>
        /// Сумма по тарифу решения
        /// </summary>
        public virtual decimal DebtDecisionTariffSum { get; set; }

        /// <summary>
        /// Количество дней просрочки
        /// </summary>
        public virtual int ExpirationDaysCount { get; set; }

        /// <summary>
        /// Количество месяцев просрочки
        /// </summary>
        public virtual int? ExpirationMonthCount { get; set; }

        /// <summary>
        /// Сумма пени
        /// </summary>
        public virtual decimal PenaltyDebt { get; set; }

        /// <summary>
        /// Наличие ПИР
        /// </summary>
        public virtual bool HasClaimWork { get; set; }

        /// <summary>
        /// Тип суда
        /// </summary>
        public virtual CourtType CourtType { get; set; }

        /// <summary>
        /// Суд
        /// </summary>
        public virtual string JurInstitution { get; set; }

        /// <summary>
        /// Пользователь, начавший ПИР
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Id дома
        /// </summary>
        public virtual long RealityObjectId { get; set; }

        /// <summary>
        /// Площадь в собственности
        /// </summary>
        public virtual decimal OwnerArea { get; set; }

        /// <summary>
        /// Несовершеннолетний
        /// </summary>
        public virtual bool Underage { get; set; }

        /// <summary>
        /// Наличие выписки из ЕГРН
        /// </summary>
        public virtual YesNo ExtractExists { get; set; }

        /// <summary>
        /// Сопоставлена ли выписка с ЛС
        /// </summary>
        public virtual YesNo AccountRosregMatched { get; set; }

        /// <summary>
        /// Обрабатывается ли ЛС агентом
        /// </summary>
        public virtual YesNo ProcessedByTheAgent { get; set; }

        /// <summary>
        /// Площадь помещения
        /// </summary>
        public virtual decimal RoomArea { get; set; }

        /// <summary>
        /// Долевая собственность
        /// </summary>
        public virtual YesNo Separate { get; set; }

        /// <summary>
        /// Id ПИР
        /// </summary>
        public virtual long? ClaimworkId { get; set; }

        /// <summary>
        /// Сумма долга по последней ПИР
        /// </summary>
        public virtual decimal LastClwDebt { get; set; }

        /// <summary>
        /// Сумма оплат после последней ПИР
        /// </summary>
        public virtual decimal PaymentsSum { get; set; }

        /// <summary>
        /// Новая сумма долга
        /// </summary>
        public virtual decimal MewClaimDebt { get; set; }

        /// <summary>
        /// Последний период в ПИР
        /// </summary>
        public virtual string LastPirPeriod { get; set; }
    }
}
