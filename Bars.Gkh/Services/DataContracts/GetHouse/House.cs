namespace Bars.Gkh.Services.DataContracts.HouseSearch
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "House")]
    public class House
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "HouseNum")]
        public string HouseNum { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Street")]
        public string Street { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "City")]
        public string City { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "IdMO")]
        public long IdMo { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }
    }
}