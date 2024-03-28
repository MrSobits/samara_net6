namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "RepairPlan")]
    public class RepairPlan
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
        [XmlAttribute(AttributeName = "Cost")]
        public string Cost { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DateStart")]
        public string DateStart { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DateEnd")]
        public string DateEnd { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FactCost")]
        public string FactCost { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ReasonReject")]
        public string ReasonReject { get; set; }
    }
}