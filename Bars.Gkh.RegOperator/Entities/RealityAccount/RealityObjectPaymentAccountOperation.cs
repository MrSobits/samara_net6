namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Операция по счету оплат дома
    /// </summary>
    public class RealityObjectPaymentAccountOperation : BaseImportableEntity
    {
        /// <summary>
        /// Дата операции
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// Счет оплат дома
        /// </summary>
        public virtual RealityObjectPaymentAccount Account { get; set; }

        /// <summary>
        /// Сумма операции
        /// </summary>
        public virtual decimal OperationSum { get; set; }

        /// <summary>
        /// Тип операции
        /// </summary>
        public virtual PaymentOperationType OperationType { get; set; }

        /// <summary>
        /// Статус операции
        /// </summary>
        public virtual OperationStatus OperationStatus { get; set; }
    }
}