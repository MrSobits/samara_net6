namespace Bars.GkhGji.Entities
{
    using System;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Период отопительного сезона
    /// </summary>
    public class HeatSeasonPeriodGji : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }
    }
}