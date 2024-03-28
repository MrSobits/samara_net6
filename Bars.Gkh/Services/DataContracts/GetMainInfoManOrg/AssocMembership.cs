namespace Bars.Gkh.Services.DataContracts.GetMainInfoManOrg
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "AssocMembership")]
    public class AssocMembership
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AsMembSite")]
        public string AsMembSite { get; set; }
    }
}