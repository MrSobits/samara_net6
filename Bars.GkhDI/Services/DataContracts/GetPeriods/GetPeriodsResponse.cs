namespace Bars.GkhDi.Services.DataContracts.GetPeriods
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.Gkh.Services.DataContracts;

    [DataContract]
    [XmlRoot(ElementName = "GetPeriodsResponse")]
    public class GetPeriodsResponse
    {
        [DataMember]
        [XmlArray(ElementName = "Periods")]
        public Period[] Periods { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}