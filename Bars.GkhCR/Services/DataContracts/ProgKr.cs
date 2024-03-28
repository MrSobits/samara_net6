namespace Bars.GkhCr.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "ProgKr")]
    public class ProgKr
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name{ get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Period")]
        public string Period { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "StartDate")]
        public string StartDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FinishDate")]
        public string FinishDate { get; set; }

        [DataMember]
        [XmlElement(ElementName = "HouseKr")]
        public HouseKr[] HousesKr { get; set; }
    }
}