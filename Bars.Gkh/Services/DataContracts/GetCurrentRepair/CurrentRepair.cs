namespace Bars.Gkh.Services.DataContracts.CurrentRepair
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "CurrentRepair")]
    public class CurrentRepair
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Measure")]
        public string Measure { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PlanSize")]
        public decimal PlanSize { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PlanSum")]
        public decimal PlanSum { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PlanDate")]
        public string PlanDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FactSize")]
        public decimal FactSize { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FactSum")]
        public decimal FactSum { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FactDate")]
        public string FactDate { get; set; }
    }
}