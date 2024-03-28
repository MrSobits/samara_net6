namespace Bars.GkhCr.Services.DataContracts.GetCrDocuments
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Секция
    /// </summary>
    public class Section
    {
        /// <summary>
        /// Id Секции
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Имя секции
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Документы
        /// </summary>
        /// [DataMember]
        [XmlElement(ElementName = "Documents")]
        public Document[] Documents { get; set; }
    }
}