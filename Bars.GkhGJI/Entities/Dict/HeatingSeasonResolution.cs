namespace Bars.GkhGji.Entities.Dict
{
    using System;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Постановления о пуске тепла
    /// </summary>
    public class HeatingSeasonResolution : BaseEntity
    {
        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Дата принятия постановления о пуске тепла
        /// </summary>
        public virtual DateTime AcceptDate { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual FileInfo Doc { get; set; }
        
        /// <summary>
        /// Период отопительного сезона
        /// </summary>
        public virtual HeatSeasonPeriodGji HeatSeasonPeriodGji { get; set; }
    }
}
