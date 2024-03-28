namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Bars.Gkh.Services.DataContracts;

    [DataContract]
    [XmlRoot(ElementName = "GetPlanWorkServiceRepairResponse")]
    public class GetPlanWorkServiceRepairResponse
    {
        [DataMember]
        [XmlArray(ElementName = "RepairPlans")]
        public RepairPlan[] RepairPlans { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}