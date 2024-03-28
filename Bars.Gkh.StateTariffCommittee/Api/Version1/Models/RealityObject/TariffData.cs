namespace Bars.Gkh.StateTariffCommittee.Api.Version1.Models.RealityObject
{
    using Bars.Gkh.StateTariffCommittee.Api.Version1.Models.Common;

    using Newtonsoft.Json;

    /// <summary>
    /// Данные по тарифам
    /// </summary>
    public class TariffData
    {
        /// <summary>
        /// Код ФИАС дома
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
        [JsonIgnore]
        public long RoomId { get; set; }

        /// <summary>
        /// Наименование поставщика коммунальных услуг
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// ИНН поставщика коммунальных услуг
        /// </summary>
        public string Inn { get; set; }

        /// <summary>
        /// КПП поставщика коммунальных услуг
        /// </summary>
        public string Kpp { get; set; }

        /// <summary>
        /// Наименование услуги
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// Тариф для населения
        /// </summary>
        public decimal Tariff { get; set; }

        /// <summary>
        /// Расчет по прибору учета
        /// </summary>
        public bool MetricCalculation { get; set; }

        /// <summary>
        /// Индивидуальный объем потребления (при наличии приборов учета)
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// Индивидуальный норматив потребления (при отсутствии приборов учета)
        /// </summary>
        public decimal Standard { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public DictIdEntry Unit { get; set; }

        #region Unit
        /// <summary>
        /// Уникальный идентификатор единицы измерения
        /// </summary>
        [JsonIgnore]
        public long UnitId
        {
            set
            {
                if (this.Unit == null)
                {
                    this.Unit = new DictIdEntry();
                }

                this.Unit.Id = value;
            }
        }

        /// <summary>
        /// Наименование единицы измерения
        /// </summary>
        [JsonIgnore]
        public string UnitName
        {
            set
            {
                if (this.Unit == null)
                {
                    this.Unit = new DictIdEntry();
                }

                this.Unit.Name = value;
            }
        }
        #endregion
    }
}