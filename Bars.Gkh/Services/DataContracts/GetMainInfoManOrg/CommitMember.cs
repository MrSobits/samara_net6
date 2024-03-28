namespace Bars.Gkh.Services.DataContracts.GetMainInfoManOrg
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "CommitMember")]
    public class CommitMember
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    }
}