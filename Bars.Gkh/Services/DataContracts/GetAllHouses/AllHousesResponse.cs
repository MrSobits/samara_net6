namespace Bars.Gkh.Services.DataContracts.GetAllHouses
{
    using Bars.Gkh.Services.DataContracts;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "AllHousesResponse")]
    public class AllHousesResponse
    {
        [DataMember]
        [XmlArray(ElementName = "AllHouse")]
        public AllHouses[] AllHouses { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}