namespace Bars.Gkh.RegOperator.Entities
{
    using System;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Поступление оплат аренды
    /// </summary>
    public class RentPaymentIn : BaseImportableEntity
    {
        /// <summary>
        /// Счет
        /// </summary>
        public virtual BasePersonalAccount Account { get; set; }

        /// <summary>
        /// Дата операции
        /// </summary>
        public virtual DateTime OperationDate { get; set; }

        /// <summary>
        /// Guid
        /// </summary>
        public virtual string Guid { get; set; }

        /// <summary>
        /// Сумма оплаты
        /// </summary>
        public virtual decimal Sum { get; set; }
    }
}