namespace Bars.Gkh.Services.DataContracts.HouseSearch
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "City")]
    public class City
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    }
}