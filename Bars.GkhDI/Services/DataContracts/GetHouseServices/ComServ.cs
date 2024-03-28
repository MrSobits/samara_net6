namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "ComServ")]
    public class ComServ
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "NameService")]
        public string NameService { get; set; }

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
        [XmlAttribute(AttributeName = "NameResOrg")]
        public string NameResOrg { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "TypeUtilities")]
        public string TypeUtilities { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ResSize")]
        public string ResSize { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Tariff")]
        public string Tariff { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DataNum")]
        public string DataNum { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DataDate")]
        public string DataDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "TarifSetOrg")]
        public string TarifSetOrg { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "TypeComServ")]
        public string TypeComServ { get; set; }
    }
}