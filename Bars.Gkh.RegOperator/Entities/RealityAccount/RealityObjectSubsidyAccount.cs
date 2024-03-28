namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Счет cубсидий дома
    /// </summary>
    public class RealityObjectSubsidyAccount : BaseImportableEntity, IRealityObjectAccount
    {
        /// <summary>
        /// Номер счета
        /// </summary>
        public virtual string AccountNumber { get; set; }

        /// <summary>
        /// Дата открытия счета
        /// </summary>
        public virtual DateTime DateOpen { get; set; }
 
        /// <summary>
        /// Объект недвижимости
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}