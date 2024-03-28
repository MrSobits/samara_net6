namespace Bars.Gkh.RegOperator.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.Gkh.Services.DataContracts;

    [DataContract]
    [XmlType(TypeName = "GetChargePeriodsResponse")]
    public class GetChargePeriodsResponse
    {
        [DataMember]
        [XmlArray(ElementName = "ChargePeriods")]
        public ChargePeriodProxy[] ChargePeriods { get; set; }
        
        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}