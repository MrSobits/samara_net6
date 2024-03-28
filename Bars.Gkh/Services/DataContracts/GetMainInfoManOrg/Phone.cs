namespace Bars.Gkh.Services.DataContracts.GetMainInfoManOrg
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "Phone")]
    public class Phone
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Number")]
        public string Number { get; set; }
    }
}