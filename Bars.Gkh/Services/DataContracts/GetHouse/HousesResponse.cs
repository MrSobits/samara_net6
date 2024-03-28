namespace Bars.Gkh.Services.DataContracts.HouseSearch
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "HousesResponse")]
    public class HousesResponse
    {
        [DataMember]
        [XmlArray(ElementName = "Houses")]
        public House[] Houses { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}