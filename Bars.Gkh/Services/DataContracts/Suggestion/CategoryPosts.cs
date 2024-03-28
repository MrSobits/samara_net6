namespace Bars.Gkh.Services.DataContracts.Suggestion
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "CategoryPosts")]
    public class CategoryPosts
    {
        [DataMember]
        [XmlElement(ElementName = "Id")]
        public long? Id { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Code")]
        public string Code { get; set; }
    }
}