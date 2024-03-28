namespace Bars.GkhCr.Services.DataContracts.GetCrDocuments
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Получение документов по объекту КР
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "CrDocumentsResponse")]
    public class CrDocumentsResponse
    {
        /// <summary>
        /// ДПКР
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Sections")]
        public Section[] Sections { get; set; }

        /// <summary>
        /// Результат 
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}