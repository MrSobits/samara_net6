namespace Bars.Gkh.Services.DataContracts.GetMkdOrgInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "GetMkdOrgInfoResponse")]
    public class GetMkdOrgInfoResponse
    {
        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }

        [DataMember]
        [XmlElement(ElementName = "UO")]
        public ManagementOrganization ManagementOrganization { get; set; }

        [DataMember]
        [XmlArray(ElementName = "PKU")]
        public SupplyResourceOrg[] SupplyResourceOrgs { get; set; }

        [DataMember]
        [XmlArray(ElementName = "PZHU")]
        public ServiceOrganization[] ServiceOrganizations { get; set; }

        [DataMember]
        [XmlArray(ElementName = "RSO")]
        public PublicServiceOrg[] PublicServiceOrgs { get; set; }
    }
}