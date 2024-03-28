namespace Bars.Gkh.Domain.PlotBuilding.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// График
    /// </summary>
    public class Serie : ISerie
    {
        /// <inheritdoc />
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <inheritdoc />
        [JsonProperty("data")]
        public object[][] Data { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="name">Наименование графика</param>
        public Serie(string name)
        {
            this.Name = name;
        }
    }
}