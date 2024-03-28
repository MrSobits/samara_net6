namespace Bars.Gkh.RegOperator.Entities.Dict
{
    using System;
    
    using Bars.Gkh.Entities;

    /// <summary>
    /// Справочник "Федеральный стандарт взноса на КР"
    /// </summary>
    public class FederalStandardFeeCr : BaseImportableEntity
    {
        /// <summary>
        /// Дата начала действия
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        /// Дата окончания действия
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public virtual decimal Value { get; set; }
    }
}