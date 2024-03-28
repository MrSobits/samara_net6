namespace Bars.Gkh.Services.DataContracts.HouseSearch
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "Flat")]
    public class Flat
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FlatNum")]
        public string FlatNum { get; set; }
    }
}