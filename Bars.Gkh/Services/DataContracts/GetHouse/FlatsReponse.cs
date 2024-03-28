namespace Bars.Gkh.Services.DataContracts.HouseSearch
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "FlatsResponse")]
    public class FlatsResponse
    {
        [DataMember]
        [XmlArray(ElementName = "Flats")]
        public Flat[] Flats { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}