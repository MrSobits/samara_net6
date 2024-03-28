namespace Bars.GkhGji.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Сведения об отмене обращения граждан
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "CitizensAppealCancelResult")]
    public class CitizensAppealCancelResult
    {
        /// <summary>
        /// UID обращения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "AppealUid")]
        public string AppealUid { get; set; }

        /// <summary>
        /// Если статус обращения = «Отменено», то отправить значение true, иначе false.
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "IsAppealСancel")]
        public bool IsAppealСancel { get; set; }

        /// <summary>
        /// Дополнительная информация. Элемент передается только если IsUploaded = false
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "AdditionalInformation")]
        public string AdditionalInformation { get; set; }
    }
}
