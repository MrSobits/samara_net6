namespace Bars.GkhDi.Entities
{
    using System;
    using Bars.Gkh.Entities;

    /// <summary>
    /// ППР список работ
    /// </summary>
    public class WorkRepairList : BaseGkhEntity
    {
        /// <summary>
        /// Услуга
        /// </summary>
        public virtual BaseService BaseService { get; set; }

        /// <summary>
        /// Группа работ по ППР
        /// </summary>
        public virtual GroupWorkPpr GroupWorkPpr { get; set; }

        /// <summary>
        /// Запланированный объем
        /// </summary>
        public virtual decimal? PlannedVolume { get; set; }

        /// <summary>
        /// Запланированная стоимость
        /// </summary>
        public virtual decimal? PlannedCost { get; set; }

        /// <summary>
        /// Фактический объем
        /// </summary>
        public virtual decimal? FactVolume { get; set; }

        /// <summary>
        /// Фактический стоимость
        /// </summary>
        public virtual decimal? FactCost { get; set; }

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
