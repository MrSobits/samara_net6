namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Информация о файле
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "FileInfo")]
    public class FileInfo
    {
        /// <summary>
        /// Наименование файла (без расширения)
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Расширение
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Extention")]
        public string Extention { get; set; }

        /// <summary>
        /// Идентификатор файла
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "IdFile")]
        public string IdFile { get; set; }
    }
}