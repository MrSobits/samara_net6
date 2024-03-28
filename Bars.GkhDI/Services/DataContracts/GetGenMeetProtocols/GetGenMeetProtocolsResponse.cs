namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Bars.Gkh.Services.DataContracts;
    using Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo;

    [DataContract]
    [XmlRoot(ElementName = "GetGenMeetProtocolsResponse")]
    public class GetGenMeetProtocolsResponse
    {
        [DataMember]
        [XmlArray(ElementName = "ManagingOrgs")]
        public ManagingOrgItem[] ManagingOrgItems { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}