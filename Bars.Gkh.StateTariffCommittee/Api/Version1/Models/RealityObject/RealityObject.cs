namespace Bars.Gkh.StateTariffCommittee.Api.Version1.Models.RealityObject
{
    using System.Collections.Generic;

    using Bars.Gkh.StateTariffCommittee.Api.Version1.Models.Common;

    using Newtonsoft.Json;

    /// <summary>
    /// Модель объекта жилищного фонда в разрезе помещений потребителей КУ
    /// </summary>
    public class RealityObjectInConsumerContext : BaseRealityObject<PremiseInConsumerContext>
    {
        /// <summary>
        /// Признак наличия прибора учета
        /// </summary>
        public bool SignMeteringDevice { get; set; }

        /// <summary>
        /// Параметры объекта
        /// </summary>
        public Options Options { get; set; }

        /// <summary>
        /// Комнаты
        /// </summary>
        public IEnumerable<RoomInConsumerContext> Rooms { get; set; }

        /// <summary>
        /// Площадь
        /// </summary>
        public decimal? Square { get; set; }

        /// <summary>
        /// Количество проживающих
        /// </summary>
        public int? NumberResidents { get; set; }
    }

    /// <summary>
    /// Модель объекта жилищного фонда в разрезе тарифов помещений потребителей КУ
    /// </summary>
    public class RealityObjectInConsumerTariffContext : BaseRealityObject<PremiseInConsumerTariffContext>
    {
        /// <summary>
        /// Данные по тарифам по жилому дому
        /// </summary>
        [JsonProperty(Order = 0)]
        public IEnumerable<TariffData> TariffData { get; set; }

        /// <summary>
        /// Комнаты
        /// </summary>
        [JsonProperty(Order = 2)]
        public IEnumerable<RoomInConsumerTariffContext> Rooms { get; set; }
    }

    /// <summary>
    /// Модель объекта жилищного фонда
    /// </summary>
    public class RealityObject : BaseRealityObject<BasePremise>
    {
        /// <summary>
        /// Уникальный идентификатор помещения
        /// </summary>
        [JsonIgnore]
        public long PremiseId { get; set; }

        /// <summary>
        /// Номер помещения дома
        /// </summary>
        [JsonIgnore]
        public string PremiseNum { get; set; }

        /// <summary>
        /// Код справочника "Категория помещения"
        /// </summary>
        [JsonIgnore]
        public string PremiseTypeCode { get; set; }

        /// <summary>
        /// Наименование справочника "Категория помещения"
        /// </summary>
        [JsonIgnore]
        public string PremiseTypeName { get; set; }

        /// <summary>
        /// Этажность
        /// </summary>
        public int Floors { get; set; }
    }

    /// <summary>
    /// Базовая модель объекта жилищного фонда
    /// </summary>
    public class BaseRealityObject<TPremise>
    {
        /// <summary>
        /// Код ФИАС дома
        /// </summary>
        public string HouseGuid { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public string Municipality { get; set; }

        /// <summary>
        /// Наименование населенного пункта
        /// </summary>
        public string Locality { get; set; }

        /// <summary>
        /// Наименование улица, проспекта, проезда, переулка и т.п.
        /// </summary>
        public string Ulica { get; set; }

        /// <summary>
        /// Номер дома
        /// </summary>
        public string HouseNum { get; set; }

        /// <summary>
        /// Строение
        /// </summary>
        public string BuildNum { get; set; }

        /// <summary>
        /// Строение
        /// </summary>
        public string StrucNum { get; set; }

        /// <summary>
        /// Тип дома
        /// </summary>
        public DictCodeEntry HouseType { get; set; }

        #region HouseType
        /// <summary>
        /// Код справочника "Типы домов"
        /// </summary>
        [JsonIgnore]
        public string HouseTypeCode
        {
            set
            {
                if (this.HouseType == null)
                {
                    this.HouseType = new DictCodeEntry();
                }

                this.HouseType.Code = value;
            }
        }

        /// <summary>
        /// Наименование справочника "Типы домов"
        /// </summary>
        [JsonIgnore]
        public string HouseTypeName
        {
            set
            {
                if (this.HouseType == null)
                {
                    this.HouseType = new DictCodeEntry();
                }

                this.HouseType.Name = value;
            }
        }
        #endregion

        /// <summary>
        /// Помещения дома
        /// </summary>
        [JsonProperty(Order = 1)]
        public IEnumerable<TPremise> Premises { get; set; }
    }
}