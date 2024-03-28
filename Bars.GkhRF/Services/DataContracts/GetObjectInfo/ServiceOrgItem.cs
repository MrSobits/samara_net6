namespace Bars.GkhRf.Services.DataContracts.GetObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "ServiceOrgItem")]
    public class ServiceOrgItem
    {
        /// <summary>
        /// ИдентификаторДоговора
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "IdDoc")]
        public long IdDoc { get; set; }

        /// <summary>
        /// Идентификатор
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// ЮридическийАдрес
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "JurAddress")]
        public string JurAddress { get; set; }

        /// <summary>
        /// ФИОРуководителя
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "FioDirector")]
        public string FioDirector { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Phone")]
        public string Phone { get; set; }
    }
}