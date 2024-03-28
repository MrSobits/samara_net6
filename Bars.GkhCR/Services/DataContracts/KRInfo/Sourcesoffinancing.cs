namespace Bars.GkhCr.Services.DataContracts.KRInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "SourcesOfFinancing")]
    public class SourcesOfFinancing
    {
        [DataMember]
        [XmlAttribute(AttributeName = "IdOfSourceFinancing")]
        public long IdOfSourceFinancing { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "NameOfFinancing")]
        public string NameOfFinancing { get; set; }
    }
}