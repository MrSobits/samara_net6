namespace Bars.Gkh.StateTariffCommittee.Api.Version1.Models.RealityObject
{
    using Newtonsoft.Json;

    /// <summary>
    /// Параметры объекта
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Guid дома
        /// </summary>
        [JsonIgnore]
        public string HouseGuid { get; set; }

        /// <summary>
        /// Идентификатор помещения
        /// </summary>
        [JsonIgnore]
        public long PremiseId { get; set; }

        /// <summary>
        /// Идентификатор комнаты
        /// </summary>
        [JsonIgnore]
        public long RoomId { get; set; }

        /// <summary>
        /// Признак наличия электрической плиты
        /// </summary>
        public bool? SignElectricStove { get; set; }

        /// <summary>
        /// Признак наличия газовой плиты
        /// </summary>
        public bool? SignGasStove { get; set; }

        /// <summary>
        /// Признак наличия газовой колонки
        /// </summary>
        public bool? SignGasColumn { get; set; }

        /// <summary>
        /// Признак наличия огневой плиты
        /// </summary>
        public bool? SignFireStove { get; set; }
    }
}