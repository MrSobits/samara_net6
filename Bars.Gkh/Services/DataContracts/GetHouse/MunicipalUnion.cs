namespace Bars.Gkh.Services.DataContracts.HouseSearch
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "MunicipalUnion")]
    public class MunicipalUnion
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "TypeMU")]
        public string TypeMU { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PresenceStreet")]
        public string PresenceStreet { get; set; }

		[DataMember]
		[XmlAttribute(AttributeName = "Parent")]
		public string Parent { get; set; }
    }
}