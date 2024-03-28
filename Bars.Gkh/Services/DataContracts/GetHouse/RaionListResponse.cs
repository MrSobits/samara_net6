namespace Bars.Gkh.Services.DataContracts.HouseSearch
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "RaionResponse")]
    public class RaionListResponse
    {
        [DataMember]
        [XmlArray(ElementName = "MunicipalUnions")]
        public MunicipalUnion[] MunicipalUnions { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}