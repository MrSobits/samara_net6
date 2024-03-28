namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class DocumentProxy
    {
        [DataMember]
        [XmlAttribute(AttributeName = "IdFile")]
        public long IdFile { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "NameFile")]
        public string NameFile { get; set; }
    }
}
