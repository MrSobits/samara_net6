namespace Bars.Gkh.Services.DataContracts.GetHousesPhotoInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Файл
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "Photo")]
    public class Photo
    {
        /// <summary>
        /// Id файла
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PhotoId")]
        public long Id { get; set; }
        
        /// <summary>
        /// Дата изображения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DatePhoto")]
        public string DatePhoto { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "NamePhoto")]
        public string NamePhoto { get; set; }

        /// <summary>
        /// Группа
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "GroupPhoto")]
        public string ImageGroup { get; set; }
        
        /// <summary>
        /// Период
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Period")]
        public string Period { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ViewWork")]
        public string ViewWork { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DescriptionPhoto")]
        public string Description { get; set; }


    }
}