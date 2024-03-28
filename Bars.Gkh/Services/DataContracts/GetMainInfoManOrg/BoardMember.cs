namespace Bars.Gkh.Services.DataContracts.GetMainInfoManOrg
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "BoardMember")]
    public class BoardMember
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    }
}