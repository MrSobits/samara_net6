namespace Bars.Gkh.Enums.EfficiencyRating
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип диаграммы
    /// </summary>
    public enum DiagramType
    {
        /// <summary>
        /// Линейная диаграмма
        /// </summary>
        [Display("Линейная диаграмма")]
        LineDiagram = 10,

        /// <summary>
        /// Столбиковая диаграмма
        /// </summary>
        [Display("Столбиковая диаграмма")]
        BarDiagram = 20,

        /// <summary>
        /// Логарифмическая диаграмма
        /// </summary>
        [Display("Логарифмическая диаграмма")]
        LogarithmicChart = 30,

        /// <summary>
        /// Секторная диаграмма
        /// </summary>
        [Display("Секторная диаграмма")]
        PieGraph = 40
    }
}