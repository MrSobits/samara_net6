namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "NonResidentialPlaceItem")]
    public class NonResidentialPlaceItem
    {
        /// <summary>
        /// НаименованиеКонтрагента
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ContragentName")]
        public string ContragentName { get; set; }

        /// <summary>
        /// ПлощадьПомещения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Area")]
        public string Area { get; set; }

        /// <summary>
        /// ТипКонтрагента
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TypeContragentDi")]
        public string TypeContragentDi { get; set; }

        /// <summary>
        /// НомерПредоставленияЖилищныхУслуг
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DocumentNumApartment")]
        public string DocumentNumApartment { get; set; }

        /// <summary>
        /// НомерПредоставленияКомунальныхУслуг
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DocumentNumCommunal")]
        public string DocumentNumCommunal { get; set; }

        /// <summary>
        /// ДатаПредоставленияКомунальныхУслуг
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DocumentDateCommunal")]
        public string DocumentDateCommunal { get; set; }

        /// <summary>
        /// ДатаПредоставленияЖилищныхУслуг
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DocumentDateApartment")]
        public string DocumentDateApartment { get; set; }

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
        /// ПриборыУчета
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "NonResidentialPlaceMetering")]
        public NonResidentialPlaceMetering[] NonResidentialPlaceMetering { get; set; }

    }
}