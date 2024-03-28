namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "InformationOnContItem")]
    public class InformationOnContItem
    {
        /// <summary>
        /// Наименование
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Стоимость
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Cost")]
        public string Cost { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Number")]
        public string Number { get; set; }

        /// <summary>
        /// Стороны
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PartiesContract")]
        public string PartiesContract { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Comments")]
        public string Comments { get; set; }

        /// <summary>
        /// ДатаЗаключенияДоговора
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "From")]
        public string From { get; set; }

        /// <summary>
        /// ДатаНачалаДоговора
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DateStart")]
        public string DateStart { get; set; }

        /// <summary>
        /// ДатаОкончанияДоговора
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DateEnd")]
        public string DateEnd { get; set; }
    }
}