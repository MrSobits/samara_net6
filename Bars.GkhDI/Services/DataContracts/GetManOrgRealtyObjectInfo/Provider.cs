namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Поставщик
    /// </summary>
    public class Provider
    {
        /// <summary>
        /// Поставщик
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ProviderName")]
        public string ProviderName { get; set; }

        /// <summary>
        /// Номер договора
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "SupplyContractNumber")]
        public string SupplyContractNumber { get; set; }

        /// <summary>
        /// Дата заключения договора
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "SupplyContractDate")]
        public DateTime SupplyContractDate { get; set; }
    }
}