namespace Bars.GkhRf.Services.DataContracts.GetObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "ManagingOrgItem")]
    public class ManagingOrgItem
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Участник
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "IdManOrg")]
        public long IdManOrg { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "IdCon")]
        public long IdCon { get; set; }

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

        /// <summary>
        /// ДатаНачалаРаботы
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "StartDate")]
        public string StartDate { get; set; }

        /// <summary>
        /// ДатаОкончанияРаботы
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "FinishDate")]
        public string FinishDate { get; set; }

        /// <summary>
        /// ТипДоговора
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ContractType")]
        public string ContractType { get; set; }

        /// <summary>
        /// ДатаНачала
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DateStart")]
        public string DateStart { get; set; }

        /// <summary>
        /// ДатаОкончания
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DateEnd")]
        public string DateEnd { get; set; }

        /// <summary>
        /// ДействующийРуководительУК
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "CurrentDirector")]
        public CurrentDirector CurrentDirector { get; set; }
    }
}