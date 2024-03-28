namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    public class HouseCommunalService
    {
        /// <summary>
        /// Вид коммунальной услуги
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Type")]
        public string Type { get; set; }

        /// <summary>
        /// Тип предоставления услуги
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "FillingFact")]
        public string FillingFact { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "MeasurementUnitsServiсу")]
        public string MeasurementUnitsServiсу { get; set; }

        /// <summary>
        /// Цена закупаемых ресурсов (тариф)
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Costs")]
        public decimal Costs { get; set; }

        /// <summary>
        /// Поставщики
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "Provider")]
        public Provider[] Provider { get; set; }

        /// <summary>
        /// (Нормативы потребления) Норматив потребления коммунальной услуги в жилых помещениях
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ConsumptionNorm")]
        public decimal ConsumptionNorm { get; set; }

        /// <summary>
        /// (Нормативы потребления) Единица измерения норматива потребления коммунальной услуги
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ConsumptionNormUnitOfMeasurement")]
        public string ConsumptionNormUnitOfMeasurement { get; set; }

        /// <summary>
        /// Поставщики
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "ProviConsumptionNormsder")]
        public ConsumptionNorms[] ConsumptionNorms { get; set; }

        /// <summary>
        /// Тариф
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Cost")]
        public decimal? Cost { get; set; }

        /// <summary>
        /// Дата начала действия тарифа
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DateStart")]
        public DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания действия тарифа
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DateEnd")]
        public DateTime? DateEnd { get; set; }

        /// <summary>
        /// Объем закупаемых ресурсов
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "VolumePurchasedResources")]
        public decimal? VolumePurchasedResources { get; set; }
    }
}