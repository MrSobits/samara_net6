namespace Bars.GkhCr.Services.DataContracts.GetCrDocuments
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Документа объекта КР
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Id документа
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DocumentId")]
        public long DocumentId { get; set; }

        /// <summary>
        /// Id вида работ
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "TypeWorkCrId")]
        public long TypeWorkCrId { get; set; }

        /// <summary>
        /// Наименование документа
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DocumentName")]
        public string DocumentName { get; set; }

        /// <summary>
        /// Id Документа
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "File")]
        public string File { get; set; }

        /// <summary>
        /// Наименование файла
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Расширение файла
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Extention")]
        public string Extention { get; set; }
    }
}