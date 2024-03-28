namespace Bars.Gkh.Gis.Entities.Register.MultipleAnalysis
{
    using B4.DataAccess;
    using IndicatorServiceComparison;

    /// <summary>
    /// Шаблон отчета об анализе
    /// </summary>
    public class MultipleAnalysisIndicator : BaseEntity
    {
        /// <summary>
        /// Индикатор
        /// </summary>
        public virtual IndicatorServiceComparison IndicatorServiceComparison { get; set; }

        /// <summary>
        /// Шаблон
        /// </summary>
        public virtual MultipleAnalysisTemplate MultipleAnalysisTemplate { get; set; }

        /// <summary>
        /// Минимальное значение
        /// </summary>
        public virtual decimal? MinValue { get; set; }

        /// <summary>
        /// Максимальное значение
        /// </summary>
        public virtual decimal? MaxValue { get; set; }

        /// <summary>
        /// Процент отклонения от среднего арифметического значения
        /// </summary>
        public virtual decimal? DeviationPercent { get; set; }

        /// <summary>
        /// Точное значение
        /// </summary>
        public virtual decimal? ExactValue { get; set; }
    }
}