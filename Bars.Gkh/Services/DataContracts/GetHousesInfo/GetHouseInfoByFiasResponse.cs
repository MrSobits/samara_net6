namespace Bars.Gkh.Services.DataContracts.HousesInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "GetHouseInfoByFiasResponse")]
    public class GetHouseInfoByFiasResponse
    {
        [DataMember]
        [XmlElement(ElementName = "HouseInfo")]
        public HouseInfo HouseInfo { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}