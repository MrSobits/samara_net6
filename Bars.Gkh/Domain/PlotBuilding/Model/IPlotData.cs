namespace Bars.Gkh.Domain.PlotBuilding.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Интерфейс объекта, который можно построить в Highcharts
    /// </summary>
    public interface IPlotData
    {
        /// <summary>
        /// Тип конструктора
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Графики, построенные на одной панели
        /// </summary>
        IEnumerable<ISerie> Series { get; }
    }
}