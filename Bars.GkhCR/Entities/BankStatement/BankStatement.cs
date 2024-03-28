namespace Bars.GkhCr.Entities
{
    using System;

    using Enums;
    using Gkh.Entities;

    /// <summary>
    /// Банковская выписка
    /// </summary>
    public class BankStatement : BaseGkhEntity
    {
        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public virtual ObjectCr ObjectCr { get; set; }

        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Period Period { get; set; }

        /// <summary>
        /// Группа финансирования
        /// </summary>
        public virtual TypeFinanceGroup TypeFinanceGroup { get; set; }

        /// <summary>
        /// Бюджетный год
        /// </summary>
        public virtual int? BudgetYear { get; set; }

        /// <summary>
        /// Входящий остаток
        /// </summary>
        public virtual decimal? IncomingBalance { get; set; }

        /// <summary>
        /// Исходящий остаток
        /// </summary>
        public virtual decimal? OutgoingBalance { get; set; }

        /// <summary>
        /// Лицевой счет
        /// </summary>
        public virtual string PersonalAccount { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Последний день операции по счету
        /// </summary>
        public virtual DateTime? OperLastDate { get; set; }

        /// <summary>
        /// Дата выписки
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }
    }
}
