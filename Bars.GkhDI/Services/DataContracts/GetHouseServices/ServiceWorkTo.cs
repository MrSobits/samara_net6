namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "WorkTo")]
    public class ServiceWorkTo
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "NameServ")]
        public string NameServ { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Cost")]
        public string Cost { get; set; }

        [DataMember]
        [XmlArray(ElementName = "Works")]
        public Work[] Works { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "NameGroup")]
        public string NameGroup { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PlanCost")]
        public string PlanCost { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FactCost")]
        public string FactCost { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DateStart")]
        public string DateStart { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DateFinish")]
        public string DateFinish { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "Work")]
    public class Work
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }
    }
}