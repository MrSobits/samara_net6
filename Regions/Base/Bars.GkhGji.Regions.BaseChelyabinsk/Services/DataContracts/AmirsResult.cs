namespace Bars.GkhGji.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// AmirsResult
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "AmirsResult")]
    public class AmirsResult
    {
        /// <summary>
        /// Номер и дата протокола
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "NumDate")]
        public string NumDate { get; set; }

        /// <summary>
        /// Признак об успешной загрузке данных: Если данные загрузились успешно, то true, иначе false
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "IsUploaded")]
        public bool IsUploaded { get; set; }

        /// <summary>
        /// Дополнительная информация. Элемент передается только если IsUploaded = false
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "AdditionalInformation")]
        public string AdditionalInformation { get; set; }
    }
}
