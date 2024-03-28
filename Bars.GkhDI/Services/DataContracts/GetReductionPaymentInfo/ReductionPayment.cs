namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "ReductionPayment")]
    public class ReductionPayment
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Date")]
        public string Date { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Reason")]
        public string Reason { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Sum")]
        public string Sum { get; set; }
    }
}