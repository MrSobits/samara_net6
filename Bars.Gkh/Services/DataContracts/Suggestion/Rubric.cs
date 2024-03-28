namespace Bars.Gkh.Services.DataContracts.Suggestion
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "Rubric")]
    public class Rubric
    {
        [DataMember]
        [XmlElement(ElementName = "Id")]
        public long? Id { get; set; }
        
        [DataMember]
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
    }
}