namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "Facility")]
    public class CommonFacilityInfo
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Num")]
        public string ContractNumber { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Date")]
        public string ContractDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Sum")]
        public decimal CostContract { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string KindCommomFacilities { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Subject")]
        public string Lessee { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "StartDate")]
        public string DateStart { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "EndDate")]
        public string DateEnd { get; set; }
    }
}