namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Bars.Gkh.Services.DataContracts;

    [DataContract]
    [XmlRoot(ElementName = "GetContractsManagementResponse")]
    public class GetContractsManagementResponse
    {
        [DataMember]
        [XmlElement(ElementName = "ProjectContractDoc")]
        public DocumentProxy ProjectContractDoc { get; set; }

        [DataMember]
        [XmlElement(ElementName = "CommunalServiceDoc")]
        public DocumentProxy CommunalServiceDoc { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ServiceApartmentDoc")]
        public DocumentProxy ServiceApartmentDoc { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}