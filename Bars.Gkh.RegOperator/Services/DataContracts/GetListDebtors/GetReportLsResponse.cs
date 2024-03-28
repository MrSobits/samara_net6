namespace Bars.Gkh.RegOperator.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.Gkh.Services.DataContracts;

    [DataContract]
    [XmlType(TypeName = "GetReportLsResponse")]
    public class GetReportLsResponse
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Base64File")]
        public string Base64File { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FileName")]
        public string FileName { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}