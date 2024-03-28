namespace Bars.Gkh.Services.DataContracts.RealityObjectHousekeeper
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "GetRealityObjectHousekeeperResponse")]
    public class GetRealityObjectHousekeeperResponse
    {
        [DataMember]
        [XmlArray(ElementName = "Housekeepers")]
        public RealityObjectHousekeeperProxy[] Housekeepers { get; set; }

        [DataMember]
        [XmlElement(ElementName = "RequestResult")]
        public RequestResult RequestResult { get; set; }
    }
}