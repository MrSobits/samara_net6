namespace Bars.Gkh.Domain.PlotBuilding.Model
{
    using System.Collections.Generic;

    using Bars.Gkh.Utils.Json;

    using Newtonsoft.Json;

    /// <summary>
    /// Базовая сущность графика
    /// </summary>
    public class BasePlotData : IPlotData
    {
        /// <inheritdoc />
        [JsonProperty("type")]
        public string Type { get; }

        /// <inheritdoc />
        [JsonProperty("xFieldTitle")]
        public string XFieldTitle { get; set; }

        /// <inheritdoc />
        [JsonProperty("yFieldTitle")]
        public string YFieldTitle { get; set; }

        /// <summary>
        /// Наименование графика
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <inheritdoc />
        [JsonProperty("series")]
        [JsonConverter(typeof(ConcreteTypeConverter<List<Serie>>))]
        public IEnumerable<ISerie> Series { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="type">Тип графика</param>
        public BasePlotData(string type)
        {
            this.Type = type;
        }
    }
}