namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Bars.Gkh.Services.DataContracts;

    [DataContract]
    [XmlRoot(ElementName = "GetManContractAppResponse")]
    public class GetManContractAppResponse
    {
        [DataMember]
        [XmlElement(ElementName = "ManContractApps")]
        public ManContractApps ManContractApps { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}