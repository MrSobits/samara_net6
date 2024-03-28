namespace Bars.Gkh.Services.DataContracts.GetHousesInfoByEditDate
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "GetHousesInfoByEditDateResponse")]
    public class GetHousesInfoByEditDateResponse
    {
        [DataMember]
        [XmlArray(ElementName = "HouseInfoByEditDate")]
        public HouseInfoByEditDate[] HouseInfoByEditDate { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}