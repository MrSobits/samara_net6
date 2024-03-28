namespace Bars.GkhDi.Regions.Tatarstan.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// ППР детализация работ (Для Татарстана)
    /// </summary>
    public class WorkRepairDetailTat : WorkRepairDetail
    {
        /// <summary>
        /// Единица измерения
        /// </summary>
        public virtual UnitMeasure UnitMeasure { get; set; }

        /// <summary>
        /// Запланированный объем
        /// </summary>
        public virtual decimal? PlannedVolume { get; set; }

        /// <summary>
        /// Фактический объем
        /// </summary>
        public virtual decimal? FactVolume { get; set; }
    }
}
