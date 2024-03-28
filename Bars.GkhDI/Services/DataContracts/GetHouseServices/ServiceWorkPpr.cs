namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "WorkPpr")]
    public class ServiceWorkPpr
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "NameWork")]
        public string NameWork { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PlanSize")]
        public string PlanSize { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PlanCost")]
        public string PlanCost { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FactSize")]
        public string FactSize { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FactSum")]
        public string FactSum { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "StartDate")]
        public string StartDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FinishDate")]
        public string FinishDate { get; set; }

        [DataMember]
        [XmlArray(ElementName = "TypeWorks")]
        public TypeWork[] TypeWorks { get; set; }

        [DataMember]
        [XmlArray(ElementName = "Details")]
        public Detail[] Details { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "TypeWork")]
    public class TypeWork
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "Detail")]
    public class Detail
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PlanSize")]
        public string PlanSize { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FactSize")]
        public string FactSize { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Izm")]
        public string Izm { get; set; }
    }
}