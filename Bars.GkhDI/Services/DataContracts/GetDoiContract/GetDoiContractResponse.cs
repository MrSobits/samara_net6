namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Bars.Gkh.Services.DataContracts;

    [DataContract]
    [XmlRoot(ElementName = "GetDoiContractResponse")]
    public class GetDoiContractResponse
    {
        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }

        [DataMember]
        [XmlArray(ElementName = "CommonFacilityInfo")]
        public CommonFacilityInfo[] CommonFacilitiesInfo { get; set; }
    }
}