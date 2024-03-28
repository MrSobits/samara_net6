namespace Bars.Gkh.Services.DataContracts.GetMainInfoManOrg
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "GetMainInfoManOrgResponse")]
    public class GetMainInfoManOrgResponse
    {
        [DataMember]
        [XmlElement(ElementName = "ManagementOrganization")]
        public ManagementOrganization ManagementOrganization { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}