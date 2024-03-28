namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class ManContractApps
    {
        [DataMember]
        [XmlElement(ElementName = "ActState")]
        public DocumentProxy ActState { get; set; }

        [DataMember]
        [XmlElement(ElementName = "CatalogRepair")]
        public DocumentProxy CatalogRepair { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ReportPlanRepair")]
        public DocumentProxy ReportPlanRepair { get; set; }
    }
}