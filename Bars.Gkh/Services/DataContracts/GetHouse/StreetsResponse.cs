namespace Bars.Gkh.Services.DataContracts.HouseSearch
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "StreetsResponse")]
    public class StreetsResponse
    {
        [DataMember]
        [XmlArray(ElementName = "Streets")]
        public Street[] Streets { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}