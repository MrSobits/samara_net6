namespace Bars.Gkh.Gis.Entities.Dict
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Gis.Enum;

    using Gkh.Entities;

    /// <summary>
    /// Справочник тарифы ГИС ЖКХ
    /// </summary>
    public class GisTariffDict : BaseGkhEntity
    {
        /// <summary>
        /// Дата загрузки в ЕИAС
        /// </summary>
        public virtual DateTime? EiasUploadDate { get; set; }

        /// <summary>
        /// Дата последнего изменения в ЕИAС
        /// </summary>
        public virtual DateTime? EiasEditDate { get; set; }

        /// <summary>
        /// Муниципальный район
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        public virtual ServiceDictionary Service { get; set; }

        /// <summary>
        /// Поставщик
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Вид деятельности поставщика
        /// </summary>
        public virtual string ActivityKind { get; set; }

        /// <summary>
        /// Наименование контрагента в базовом периоде
        /// </summary>
        public virtual string ContragentName { get; set; }

        /// <summary>
        /// Дата начала периода
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания периода
        /// </summary>
        public virtual DateTime EndDate { get; set; }

        /// <summary>
        /// Вид тарифа
        /// </summary>
        public virtual GisTariffKind TariffKind { get; set; }

        /// <summary>
        /// Количество зон
        /// </summary>
        public virtual int? ZoneCount { get; set; }

        /// <summary>
        /// Значение тарифа
        /// </summary>
        public virtual decimal? TariffValue { get; set; }

        /// <summary>
        /// Значение тарифа 1
        /// </summary>
        public virtual decimal? TariffValue1 { get; set; }

        /// <summary>
        /// Значение тарифа 2
        /// </summary>
        public virtual decimal? TariffValue2 { get; set; }

        /// <summary>
        /// Значение тарифа 3
        /// </summary>
        public virtual decimal? TariffValue3 { get; set; }

        /// <summary>
        /// Включая НДС
        /// </summary>
        public virtual bool? IsNdsInclude { get; set; }

        /// <summary>
        /// В пределах социальной нормы
        /// </summary>
        public virtual bool? IsSocialNorm { get; set; }

        /// <summary>
        /// Наличие прибора учета
        /// </summary>
        public virtual bool? IsMeterExists { get; set; }

        /// <summary>
        /// Наличие электрической плиты
        /// </summary>
        public virtual bool? IsElectricStoveExists { get; set; }

        /// <summary>
        /// Этаж
        /// </summary>
        public virtual int? Floor { get; set; }

        /// <summary>
        /// Вид потребителя
        /// </summary>
        public virtual ConsumerType? ConsumerType { get; set; }

        /// <summary>
        /// Вид населенногоо пункта
        /// </summary>
        public virtual SettelmentType? SettelmentType { get; set; }

        /// <summary>
        /// Тип потребителя по электроэнергии
        /// </summary>
        public virtual ConsumerByElectricEnergyType? ConsumerByElectricEnergyType { get; set; }

        /// <summary>
        /// Дополнительный признак организации в регулируемом периоде
        /// </summary>
        public virtual string RegulatedPeriodAttribute { get; set; }

        /// <summary>
        /// Дополнительный признак организации в базовом периоде
        /// </summary>
        public virtual string BasePeriodAttribute { get; set; }
    }
}