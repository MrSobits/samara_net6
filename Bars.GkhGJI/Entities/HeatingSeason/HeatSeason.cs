namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Подготовка к отопительному сезону (не путать с проверкой по отопительному сезону)
    /// </summary>
    public class HeatSeason : BaseGkhEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Период отопительного сезона
        /// </summary>
        public virtual HeatSeasonPeriodGji Period { get; set; }

        /// <summary>
        /// Дата пуска тепла в дом
        /// </summary>
        public virtual DateTime? DateHeat { get; set; }

        /// <summary>
        /// Система отопления (по умолчанию тянется из Жилого дома - только для Татарстана!) 
        /// </summary>
        public virtual HeatingSystem HeatingSystem { get; set; }
    }
}