namespace Bars.Gkh.RegOperator.Entities.PersonalAccount
{
    using System;
    using B4.Modules.FileStorage;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Отмена оплаты
    /// </summary>
    public class CancelPayment : BaseImportableEntity
    {
        /// <summary>
        /// Лицевой счет
        /// </summary>
        public virtual BasePersonalAccount Account { get; set; }

        /// <summary>
        /// Дата операции
        /// </summary>
        public virtual DateTime OperationDate { get; set; }

        /// <summary>
        /// Сумма списания
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Документ основание
        /// </summary>
        public virtual FileInfo Document { get; set; }

        /// <summary>
        /// Причина
        /// </summary>
        public virtual string Reason { get; set; }
    }
}