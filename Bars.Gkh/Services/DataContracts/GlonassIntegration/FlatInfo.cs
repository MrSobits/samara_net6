namespace Bars.Gkh.Services.DataContracts.GlonassIntegration
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Модель информации по помещению
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "FlatInfo")]
    public class FlatInfo
    {
        /// <summary>Номер помещения</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Num")]
        public string Num { get; set; }

        /// <summary>Площадь помещения</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Area")]
        public string Area { get; set; }

        /// <summary>Тип помещения</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TypeRoom")]
        public string TypeRoom { get; set; }

        /// <summary>Тип собственности</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }

        /// <summary>Собственник/арендатор нежилого помещения</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Owner")]
        public string Owner { get; set; }

        /// <summary>Контактные данные собственника/арендатора нежилого помещения</summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Telephone")]
        public string Telephone { get; set; }

    }
}