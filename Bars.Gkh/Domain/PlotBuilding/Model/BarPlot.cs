namespace Bars.Gkh.Domain.PlotBuilding.Model
{
    /// <summary>
    /// Столбиковая диаграмма
    /// </summary>
    public class BarPlot : BasePlotData
    {
        /// <inheritdoc />
        public BarPlot() : base("column")
        {
        }
    }
}