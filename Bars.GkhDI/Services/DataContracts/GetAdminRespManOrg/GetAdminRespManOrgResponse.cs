namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Bars.Gkh.Services.DataContracts;

    [DataContract]
    [XmlRoot(ElementName = "GetAdminRespManOrgResponse")]
    public class GetAdminRespManOrgResponse
    {
        [DataMember]
        [XmlElement(ElementName = "HasAdminResp")]
        public string HasAdminResp { get; set; }

        [DataMember]
        [XmlArray(ElementName = "AdministrativeResponsibilities")]
        public AdminRespons[] AdminResponses { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}