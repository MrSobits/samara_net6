namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Приборы учёта
    /// </summary>
    public class HouseMeteringDevice
    {
        /// <summary>
        /// Вида коммунального ресурса
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "CommunalResourceType")]
        public string CommunalResourceType { get; set; }

        /// <summary>
        /// Наличия прибора учета
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Availability")]
        public string Availability { get; set; }

        /// <summary>
        /// Тип прибора учета
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "MeterType")]
        public string MeterType { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "UnitOfMeasurement")]
        public string UnitOfMeasurement { get; set; }

        /// <summary>
        /// Дата ввода в эксплуатацию
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "CommissioningDate")]
        public string CommissioningDate { get; set; }

        /// <summary>
        /// Дата поверки / замены прибора учета.
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "CalibrationDate")]
        public string CalibrationDate { get; set; }

    }
}