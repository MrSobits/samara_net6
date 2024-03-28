namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "Contract")]
    public class Contract
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "NameManOrg")]
        public string NameManOrg { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Number")]
        public string Number { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "StartDate")]
        public string StartDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FinishDate")]
        public string FinishDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Member")]
        public string Member { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Sum")]
        public string Sum { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Comments")]
        public string Comments { get; set; }
    }
}