namespace Bars.GkhRf.Services.DataContracts.GetObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "WorkItem")]
    public class WorkItem
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "IdWork")]
        public long IdWork { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// ДатаНачала
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "StartDate")]
        public string StartDate { get; set; }

        /// <summary>
        /// ПроцентВыполнения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Percent")]
        public string Percent { get; set; }

        /// <summary>
        /// ФотоАрхив
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "Image")]
        public ImageItem[] Image { get; set; }
    }
}