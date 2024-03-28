namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "InfoAboutUseCommonFacilItem")]
    public class InfoAboutUseCommonFacilItem
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// ВидОбщегоИмущества
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "KindCommomFacilities")]
        public string KindCommomFacilities { get; set; }

        /// <summary>
        /// НаименованиеАрендатора
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Lessee")]
        public string Lessee { get; set; }

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
        /// СуммаДоговора
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "CostContract")]
        public string CostContract { get; set; }

        /// <summary>
        /// НомерПротокола
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Number")]
        public string Number { get; set; }

        /// <summary>
        /// ТипДоговора
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TypeContract")]
        public string TypeContract { get; set; }

        /// <summary>
        /// ДатаПротокола
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "From")]
        public string From { get; set; }

        /// <summary>
        /// Площадь общего имущества, кв. м
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "AreaOfCommonFacilities")]
        public decimal AreaOfCommonFacilities { get; set; }
    }
}