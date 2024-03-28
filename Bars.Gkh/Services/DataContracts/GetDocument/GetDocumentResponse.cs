namespace Bars.Gkh.Services.DataContracts.GetDocument
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Gkh.Services.DataContracts;

    [DataContract]
    [XmlRoot(ElementName = "GetDocumentResponse")]
    public class GetDocumentResponse
    {
        [DataMember]
        [XmlElement(ElementName = "Document")]
        public Document Document { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}