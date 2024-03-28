namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using B4.Modules.FileStorage;

    using Bars.Gkh.Entities;

    public class CalcAccountCredit : BaseImportableEntity
    {
        /// <summary>
        /// Расчетный счет
        /// </summary>
        public virtual CalcAccount Account { get; set; }

        /// <summary>
        /// Дата формирования кредита
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        /// Дата погашения кредита
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Сумма кредита
        /// </summary>
        public virtual decimal CreditSum { get; set; }

        /// <summary>
        /// Сумма процентов
        /// </summary>
        public virtual decimal PercentSum { get; set; }

        /// <summary>
        /// Процентная ставка
        /// </summary>
        public virtual decimal PercentRate { get; set; }

        /// <summary>
        /// Срок кредита (месяцев)
        /// </summary>
        public virtual decimal CreditPeriod { get; set; }

        /// <summary>
        /// Сумма основного долга (оставшаяся)
        /// </summary>
        public virtual decimal CreditDebt { get; set; }

        /// <summary>
        /// Сумма долга по процентам
        /// </summary>
        public virtual decimal PercentDebt { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual FileInfo Document { get; set; }
    }
}