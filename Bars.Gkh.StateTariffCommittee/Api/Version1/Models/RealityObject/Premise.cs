namespace Bars.Gkh.StateTariffCommittee.Api.Version1.Models.RealityObject
{
    using System.Collections.Generic;

    using Bars.Gkh.StateTariffCommittee.Api.Version1.Models.Common;

    using Newtonsoft.Json;

    /// <summary>
    /// Помещение в разрезе потребителя КУ
    /// </summary>
    public class PremiseInConsumerContext : Premise<RoomInConsumerContext>
    {
        /// <summary>
        /// Площадь помещения
        /// </summary>
        public decimal Square { get; set; }

        /// <summary>
        /// Количество проживающих
        /// </summary>
        public int NumberResidents { get; set; }

        /// <summary>
        /// Признак наличия прибора учета
        /// </summary>
        public bool SignMeteringDevice { get; set; }

        /// <summary>
        /// Параметры помещения
        /// </summary>
        public Options Options { get; set; }
    }

    /// <summary>
    /// Помещение в разрезе тарифов потребителя КУ
    /// </summary>
    public class PremiseInConsumerTariffContext : Premise<RoomInConsumerTariffContext>
    {
        /// <summary>
        /// Данные по тарифам по помещению
        /// </summary>
        public IEnumerable<TariffData> TariffData { get; set; }
    }

    /// <summary>
    /// Основная модель помещения с указанием типа помещения
    /// </summary>
    public class Premise<TRoom> : BasePremise
    {
        /// <summary>
        /// Характеристика помещения
        /// </summary>
        public DictCodeEntry PremiseCharacteristics { get; set; }

        #region PremiseCharacteristics
        /// <summary>
        /// Код характеристики помещения
        /// </summary>
        [JsonIgnore]
        public string PremiseCharacteristicsCode
        {
            set
            {
                if (this.PremiseCharacteristics == null)
                {
                    this.PremiseCharacteristics = new DictCodeEntry();
                }

                this.PremiseCharacteristics.Code = value;
            }
        }

        /// <summary>
        /// Название характеристики помещения
        /// </summary>
        [JsonIgnore]
        public string PremiseCharacteristicsName
        {
            set
            {
                if (this.PremiseCharacteristics == null)
                {
                    this.PremiseCharacteristics = new DictCodeEntry();
                }

                this.PremiseCharacteristics.Name = value;
            }
        }
        #endregion

        /// <summary>
        /// Комнаты
        /// </summary>
        public IEnumerable<TRoom> Rooms { get; set; }
    }

    /// <summary>
    /// Базовая модель помещения
    /// </summary>
    public class BasePremise
    {
        /// <summary>
        /// GUID дома
        /// </summary>
        [JsonIgnore]
        public string HouseGuid { get; set; }

        /// <summary>
        /// Уникальный идентификатор помещения
        /// </summary>
        public long PremiseId { get; set; }

        /// <summary>
        /// Номер помещения
        /// </summary>
        public string PremiseNum { get; set; }

        /// <summary>
        /// Тип помещения
        /// </summary>
        public DictCodeEntry PremiseType { get; set; }

        #region PremiseType
        /// <summary>
        /// Код типа помещения
        /// </summary>
        [JsonIgnore]
        public string PremiseTypeCode
        {
            set
            {
                if (this.PremiseType == null)
                {
                    this.PremiseType = new DictCodeEntry();
                }

                this.PremiseType.Code = value;
            }
        }

        /// <summary>
        /// Название типа помещения
        /// </summary>
        [JsonIgnore]
        public string PremiseTypeName
        {
            set
            {
                if (this.PremiseType == null)
                {
                    this.PremiseType = new DictCodeEntry();
                }

                this.PremiseType.Name = value;
            }
        }
        #endregion
    }
}