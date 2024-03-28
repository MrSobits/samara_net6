namespace Bars.GkhDi.Services.DataContracts.HousesManOrg
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.Gkh.Services.DataContracts;

    [DataContract]
    [XmlRoot(ElementName = "GetHousesManOrgResponse")]
    public class GetHousesManOrgResponse
    {
        [DataMember]
        [XmlArray(ElementName = "HousesManOrg")]
        public House[] HousesManOrg { get; set; }

        [DataMember]
        [XmlArray(ElementName = "NoHousesManOrg")]
        public House[] NoHousesManOrg { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}