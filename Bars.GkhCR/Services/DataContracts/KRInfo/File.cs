namespace Bars.GkhCr.Services.DataContracts.KRInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Файл
    /// </summary>
    public class File
    {
        /// <summary>
        /// Id Документа
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Наименование файла
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Содержимое
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Base64")]
        public string Base64 { get; set; }

        /// <summary>
        /// Расширение файла
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Extention")]
        public string Extention { get; set; }

    }
}