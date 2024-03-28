namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "FinancialActivity")]
    public class FinancialActivity
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Address")]
        public string Address { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "TotalArea")]
        public string TotalArea { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Payment")]
        public string Payment { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "GetForServices")]
        public string GetForServices { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "SumDept")]
        public string SumDept { get; set; }
    }
}