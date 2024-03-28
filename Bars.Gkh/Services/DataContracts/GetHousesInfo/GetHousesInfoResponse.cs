namespace Bars.Gkh.Services.DataContracts.HousesInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "GetHousesInfoResponse")]
    public class GetHousesInfoResponse
    {
        [DataMember]
        [XmlArray(ElementName = "HousesInfo")]
        public HouseInfo[] HousesInfo { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}