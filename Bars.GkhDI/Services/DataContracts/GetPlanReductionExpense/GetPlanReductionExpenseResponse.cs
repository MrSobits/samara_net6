namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Bars.Gkh.Services.DataContracts;

    [DataContract]
    [XmlRoot(ElementName = "GetPlanReductionExpenseResponse")]
    public class GetPlanReductionExpenseResponse
    {
        [DataMember]
        [XmlArray(ElementName = "PlanReductionExpenses")]
        public Arrangement[] PlanReductionExpenses { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}