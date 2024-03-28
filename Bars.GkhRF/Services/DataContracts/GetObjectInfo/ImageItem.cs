namespace Bars.GkhRf.Services.DataContracts.GetObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "ImageItem")]
    public class ImageItem
    {
        /// <summary>
        /// Наименование(ФотоАрхивВходеКапремонта, ФотоАрхивДоКапремонта, ФотоАрхивПослеКапремонта)
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Наименование(ФотоАрхивВходеКапремонта, ФотоАрхивДоКапремонта, ФотоАрхивПослеКапремонта)
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Group")]
        public string Group { get; set; }

        /// <summary>
        /// Идентификатор
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// ДатаИзменения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DateChange")]
        public string DateChange { get; set; }

        /// <summary>
        /// ДатаИзображения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DateImage")]
        public string DateImage { get; set; }

        /// <summary>
        /// ИсходноеИмяФайла
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "NameFile")]
        public string NameFile { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Notation")]
        public string Notation { get; set; }

        /// <summary>
        /// Base64string
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Value")]
        public string Value { get; set; }
    }
}