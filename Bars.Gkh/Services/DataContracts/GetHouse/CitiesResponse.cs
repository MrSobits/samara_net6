namespace Bars.Gkh.Services.DataContracts.HouseSearch
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "CitiesResponse")]
    public class CitiesResponse
    {
        [DataMember]
        [XmlArray(ElementName = "Cities")]
        public City[] Cities { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}