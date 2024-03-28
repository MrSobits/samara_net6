namespace Bars.GkhRf.Services.DataContracts.GetRegOpPayment
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "Payment")]
    public class Payment
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Date")]
        public string Date { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ImpBalance")]
        public string ImpBalance { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ExpBalance")]
        public string ExpBalance { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Recalculation")]
        public string Recalculation { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Credit")]
        public string Credit { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Paid")]
        public string Paid { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "TotalArea")]
        public string TotalArea { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ManOrg")]
        public string ManOrg { get; set; }
    }
}