namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    ///  Фактическое поступление субсидий
    /// </summary>
    public class RealityObjectSubsidyAccountOperation : BaseImportableEntity
    {
        /// <summary>
        /// Счет субсидий
        /// </summary>
        public virtual RealityObjectSubsidyAccount Account { get; set; }

        /// <summary>
        /// Сумма операции
        /// </summary>
        public virtual decimal OperationSum { get; set; }

        /// <summary>
        /// Тип операции
        /// </summary>
        public virtual PaymentOperationType OperationType { get; set; }

        /// <summary>
        /// Дата операции
        /// </summary>
        public virtual DateTime Date { get; set; }
    }
}