namespace Bars.Gkh.StateTariffCommittee.Api.Version1.Models.RealityObject
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    /// <summary>
    /// Комната в разрезе потребителя КУ
    /// </summary>
    public class RoomInConsumerContext : BaseRoom
    {
        /// <summary>
        /// Площадь комнаты
        /// </summary>
        public decimal Square { get; set; }

        /// <summary>
        /// Уникальный идентификатор комнаты
        /// </summary>
        public int NumberResidents { get; set; }

        /// <summary>
        /// Признак наличия прибора учета
        /// </summary>
        public bool SignMeteringDevice { get; set; }

        /// <summary>
        /// Параметры комнаты
        /// </summary>
        public Options Options { get; set; }
    }

    /// <summary>
    /// Комната в разрезе тарифов потребителя КУ
    /// </summary>
    public class RoomInConsumerTariffContext : BaseRoom
    {
        /// <summary>
        /// Данные по тарифам по комнате
        /// </summary>
        public IEnumerable<TariffData> TariffData { get; set; }
    }

    /// <summary>
    /// Базовая модель комнаты
    /// </summary>
    public class BaseRoom
    {
        /// <summary>
        /// GUID дома
        /// </summary>
        [JsonIgnore]
        public string HouseGuid { get; set; }

        /// <summary>
        /// Уникальный идентификатор помещения
        /// </summary>
        [JsonIgnore]
        public long PremiseId { get; set; }

        /// <summary>
        /// Уникальный идентификатор комнаты
        /// </summary>
        public long RoomId { get; set; }

        /// <summary>
        /// Номер комнаты
        /// </summary>
        public string RoomNum { get; set; }
    }
}