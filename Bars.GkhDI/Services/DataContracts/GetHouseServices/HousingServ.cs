namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "HousingServ")]
    public class HousingServ
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }   
        
        [DataMember]
        [XmlAttribute(AttributeName = "NameService")]
        public string NameService { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Provider")]
        public string Provider { get; set; }
        
        [DataMember]
        [XmlAttribute(AttributeName = "Periodicity")]
        public string Periodicity { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Measure")]
        public string Measure { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Cost")]
        public string Cost { get; set; }

        [DataMember]
        [XmlElement(ElementName = "WorksTo")]
        public ServiceWorkTo WorksTo { get; set; }

        [DataMember]
        [XmlArray(ElementName = "WorksPpr")]
        public ServiceWorkPpr[] WorksPpr { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PlanCost")]
        public string PlanCost { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FactCost")]
        public string FactCost { get; set; }

    }
}