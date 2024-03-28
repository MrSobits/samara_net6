namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Bars.Gkh.Services.DataContracts;

    [DataContract]
    [XmlRoot(ElementName = "GetFinEconInfoManOrgResponse")]
    public class GetFinEconInfoManOrgResponse
    {
        [DataMember]
        [XmlArray(ElementName = "FinancialActivities")]
        public FinancialActivity[] FinancialActivities { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}