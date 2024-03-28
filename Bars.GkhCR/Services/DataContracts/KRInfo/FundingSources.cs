namespace Bars.GkhCr.Services.DataContracts.KRInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "FundingSources")]
    public class FundingSources
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FinanceSourceId")]
        public long FinanceSourceId { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "IdWork")]
        public long IdWork { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "TypeWork")]
        public string TypeWork { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Year")]
        public int Year { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "BudgetMu")]
        public decimal BudgetMu { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "BudgetSubject")]
        public decimal BudgetSubject { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "OwnerResource")]
        public decimal OwnerResource { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FundResource")]
        public decimal FundResource { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "OtherSource")]
        public decimal OtherResource { get; set; }
    }
}