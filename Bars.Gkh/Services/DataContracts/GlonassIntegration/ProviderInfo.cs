namespace Bars.Gkh.Services.DataContracts.GlonassIntegration
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Модель информации по организации-поставщику услуг ЖКХ
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "ProviderInfo")]
    public class ProviderInfo
    {
        /// <summary>Наименование организации</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ProviderName")]
        public string ProviderName { get; set; }

        /// <summary>Адрес организации</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ProviderAddress")]
        public string ProviderAddress { get; set; }

        /// <summary>Контактный телефон</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ProviderPhone")]
        public string ProviderPhone { get; set; }

        /// <summary>Телефон аварийной службы</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ProviderEmergency")]
        public string ProviderEmergency { get; set; }
    }
}