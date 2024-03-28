namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Bars.Gkh.Services.DataContracts;

    [DataContract]
    [XmlRoot(ElementName = "GetFinEconDocsManOrgResponse")]
    public class GetFinEconDocsManOrgResponse
    {
        [DataMember]
        [XmlElement(ElementName = "DocFEAct")]
        public DocFeAct DocFeAct { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}