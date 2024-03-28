namespace Bars.GkhCr.Entities
{
    using Gkh.Entities;

    /// <summary>
    /// Ведомость ресурсов
    /// </summary>
    public class ResourceStatement : BaseGkhEntity
    {
        /// <summary>
        /// Сметный расчет по работе
        /// </summary>
        public virtual EstimateCalculation EstimateCalculation { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Ед. измерения
        /// </summary>
        public virtual string UnitMeasure { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Обоснование
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        /// Количество всего
        /// </summary>
        public virtual decimal? TotalCount { get; set; }

        /// <summary>
        /// Общая стоимость
        /// </summary>
        public virtual decimal? TotalCost { get; set; }

        /// <summary>
        /// Стоимость на ед.
        /// </summary>
        public virtual decimal? OnUnitCost { get; set; }
    }
}
