namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "ManagServ")]
    public class ManagServ
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Provider")]
        public string Provider { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Measure")]
        public string Measure { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Cost")]
        public string Cost { get; set; }  
        
        [DataMember]
        [XmlAttribute(AttributeName = "NameService")]
        public string NameService { get; set; }
    }
}