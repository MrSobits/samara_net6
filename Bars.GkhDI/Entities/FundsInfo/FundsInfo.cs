namespace Bars.GkhDi.Entities
{
    using System;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Сведения о фондах
    /// </summary>
    public class FundsInfo : BaseGkhEntity
    {
        /// <summary>
        /// Сведения об УО
        /// </summary>
        public virtual DisclosureInfo DisclosureInfo { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Размер
        /// </summary>
        public virtual decimal? Size { get; set; }

        /// <summary>
        /// По состоянию на
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }
    }
}
