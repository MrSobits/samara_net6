namespace Bars.GkhGji.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Результат передачи данных об обращении
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "AppealTransferResult")]
    public class AppealTransferResult
    {
        /// <summary>
        /// UID обращения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "AppealUid")]
        public string AppealUid { get; set; }

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
