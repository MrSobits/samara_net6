namespace Bars.Gkh.Services.DataContracts.GlonassIntegration
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Модель информации по управляющей организации
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "UoInfo")]
    public class UoInfo
    {
        /// <summary>Наименование организации</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "UOName")]
        public string UOName { get; set; }

        /// <summary>Форма управления</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "UOType")]
        public string UOType { get; set; }

        /// <summary>Адрес организации</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "UOAdress")]
        public string UOAdress { get; set; }

        /// <summary>Контактный телефон</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "UOPhone")]
        public string UOPhone { get; set; }

        /// <summary>Телефон аварийной службы</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "UOEmergency")]
        public string UOEmergency { get; set; }
    }
}