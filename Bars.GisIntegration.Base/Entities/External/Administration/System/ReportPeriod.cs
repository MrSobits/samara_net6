namespace Bars.GisIntegration.Base.Entities.External.Administration.System
{
    using B4.DataAccess;

    using global::System;

    /// <summary>
    /// Поставщик информации
    /// </summary>
    public class ReportPeriod : BaseEntity
    {
        /// <summary>
        /// Поставщик информации
        /// </summary>
        public virtual DataSupplier DataSupplier { get; set; }
        
        /// <summary>
        /// Отчетный период
        /// </summary>
        public virtual DateTime? ReportMonth { get; set; }
    }
}