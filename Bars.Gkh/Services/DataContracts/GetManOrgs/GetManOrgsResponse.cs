namespace Bars.Gkh.Services.DataContracts.ManagementOrganizationSearch
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "GetManOrgsResponse")]
    public class GetManOrgsResponse
    {
        [DataMember]
        [XmlArray(ElementName = "ManagementOrganizations")]
        public ManagementOrganization[] ManagementOrganizations { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}