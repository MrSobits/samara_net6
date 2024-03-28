namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "Arrangement")]
    public class Arrangement
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Period")]
        public string Period { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PlannedReductionExpense")]
        public string PlannedReductionExpense { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FactedReductionExpense")]
        public string FactedReductionExpense { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "RejectReason")]
        public string RejectReason { get; set; }
    }
}