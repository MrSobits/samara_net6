namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Работа услуги капремонт
    /// </summary>
    public class WorkCapRepair : BaseGkhEntity
    {
        /// <summary>
        /// Услуга
        /// </summary>
        public virtual BaseService BaseService { get; set; }

        /// <summary>
        /// Работа капремонта
        /// </summary>
        public virtual Work Work { get; set; }

        /// <summary>
        /// Запланированный объем
        /// </summary>
        public virtual decimal? PlannedVolume { get; set; }

        /// <summary>
        /// Запланированная сумма
        /// </summary>
        public virtual decimal? PlannedCost { get; set; }

        /// <summary>
        /// Фактический объем
        /// </summary>
        public virtual decimal? FactedVolume { get; set; }

        /// <summary>
        /// Фактическая сумма
        /// </summary>
        public virtual decimal? FactedCost { get; set; }
    }
}