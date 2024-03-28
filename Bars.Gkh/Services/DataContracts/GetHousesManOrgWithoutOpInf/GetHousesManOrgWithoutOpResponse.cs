namespace Bars.Gkh.Services.DataContracts.GetHousesManOrgWithoutOpInf
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "GetHousesManOrgWithoutOpResponse")]
    public class GetHousesManOrgWithoutOpResponse
    {
        [DataMember]
        [XmlArray(ElementName = "HouseManOrg")]
        public HouseManOrgProxy[] HouseManOrg { get; set; }

        [DataMember]
        [XmlArray(ElementName = "NoHousesManOrg")]
        public NoHousesManOrgProxy[] NoHousesManOrg { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}