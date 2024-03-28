namespace Bars.GkhGji.Regions.Chelyabinsk.Services.DataContracts.SyncOperators
{
    using System.IO;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "GetReportResponse")]
    public class GetReportResponse
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Base64String")]
        public string Base64String { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FileName")]
        public string FileName { get; set; }

        [DataMember]
        [XmlElement(ElementName = "RequestResult")]
        public RequestResult RequestResult { get; set; }
    }
}