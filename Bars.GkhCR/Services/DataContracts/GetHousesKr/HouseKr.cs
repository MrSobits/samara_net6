namespace Bars.GkhCr.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "HouseKr")]
    public class HouseKr
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Mu")]
        public string Mu { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Address")]
        public string Address{ get; set; }
    }
}