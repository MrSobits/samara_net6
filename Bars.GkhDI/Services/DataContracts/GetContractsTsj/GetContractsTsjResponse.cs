namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Bars.Gkh.Services.DataContracts;

    [DataContract]
    [XmlRoot(ElementName = "GetContractsTsjResponse")]
    public class GetContractsTsjResponse
    {
        [DataMember]
        [XmlArray(ElementName = "Contracts")]
        public Contract[] Contracts { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}