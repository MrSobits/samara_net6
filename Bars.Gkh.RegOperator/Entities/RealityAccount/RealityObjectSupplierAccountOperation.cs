namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Gkh.Entities.Dicts;

    /// <summary>
    /// Операции счета по рассчета с поставщиками
    /// </summary>
    public class RealityObjectSupplierAccountOperation: BaseImportableEntity
    {
        /// <summary>
        /// Дата
        /// </summary>
        public virtual DateTime Date { get; set; }
        
        /// <summary>
        /// Вид опреации
        /// </summary>
        public virtual PaymentOperationType OperationType  { get; set; }
        
        /// <summary>
        /// Обороты по дебету
        /// </summary>
        public virtual decimal Debt { get; set; }

        /// <summary>
        /// Обороты по кредиту
        /// </summary>
        public virtual decimal Credit { get; set; }

        /// <summary>
        /// Счет
        /// </summary>
        public virtual RealityObjectSupplierAccount Account { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual Work Work { get; set; }
    }
}
