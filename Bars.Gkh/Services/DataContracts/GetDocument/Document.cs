namespace Bars.Gkh.Services.DataContracts.GetDocument
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "Document")]
    public class Document
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Extension")]
        public string Extension { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "File")]
        public string File { get; set; }
    }
}