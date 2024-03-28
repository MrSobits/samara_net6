namespace Bars.Gkh.Overhaul.Entities
{
    using System;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Enum;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Базовый класс Счет
    /// </summary>
    public class BankAccount : BaseImportableEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Номер счета
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Дата открытия
        /// </summary>
        public virtual DateTime OpenDate { get; set; }

        /// <summary>
        /// Дата закрытия
        /// </summary>
        public virtual DateTime? CloseDate { get; set; }

        /// <summary>
        /// Итого по расходу
        /// </summary>
        public virtual decimal? TotalOut { get; set; }

        /// <summary>
        /// Итого по приходу 
        /// </summary>
        public virtual decimal? TotalIncome { get; set; }

        /// <summary>
        /// Сальдо по счету
        /// </summary>
        public virtual decimal Balance { get; set; }

        /// <summary>
        /// Дата последней операции по счету
        /// </summary>
        public virtual DateTime? LastOperationDate { get; set; }

        /// <summary>
        /// Тип счета
        /// </summary>
        public virtual AccountType AccountType { get; set; }

        /// <summary>
        /// Лимит по кредиту
        /// </summary>
        public virtual decimal? CreditLimit { get; set; }
    }
}