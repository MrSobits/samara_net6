namespace Bars.GkhCr.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Срок по муниципальному образованию
    /// </summary>
    public class ControlDateMunicipalityLimitDate : BaseEntity
    {
        /// <summary>
        /// Программа КР
        /// </summary>
        public virtual ControlDate ControlDate { get; set; }

        /// <summary>
        /// Миниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Срок
        /// </summary>
        public virtual DateTime LimitDate { get; set; }
    }
}
