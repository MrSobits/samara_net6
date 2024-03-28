namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Счет расчета с поставщиками
    /// </summary>
    public class RealityObjectSupplierAccount : BaseImportableEntity, IRealityObjectAccount
    {
        /// <summary>
        /// Номер счета
        /// </summary>
        public virtual string AccountNumber { get; set; }

        /// <summary>
        /// Дата открытия
        /// </summary>
        public virtual DateTime OpenDate { get; set; }

        /// <summary>
        /// Дата закрытия
        /// </summary>
        public virtual DateTime CloseDate { get; set; }
        
        /// <summary>
        /// Дом - владелец счета
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}
