namespace Bars.Gkh.Services.DataContracts.DataTransfer
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "SectionProgressParams")]
    public class SectionProgressParams : DataTransferParameters
    {
        [DataMember]
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Success")]
        public bool Success { get; set; }
    }
}