namespace Bars.Gkh.Services.DataContracts.Suggestion
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "File")]
    public class File
    {
        [DataMember]
        [XmlElement(ElementName = "Id")]
        public long? Id { get; set; }

        [DataMember]
        [XmlElement(ElementName = "FileName")]
        public string FileName { get; set; }
        
        [DataMember]
        [XmlElement(ElementName = "Base64")]
        public string Base64 { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Url")]
        public string Url { get; set; }

        [DataMember]
        [XmlElement(ElementName = "IsAnswer")]
        public bool? IsAnswer { get; set; }
    }
}