namespace Bars.Gkh.Overhaul.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "PublishProgRec")]
    public class PublishProgRecWcfProxy
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Municipality")]
        public string Municipality{ get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PublishDate")]
        public string PublishDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Address")]
        public string Address { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Ceo")]
        public string Ceo { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PublishYear")]
        public string PublishYear { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Number")]
        public int Number { get; set; }
    }
}