namespace Bars.Gkh.Services.DataContracts.Suggestion
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "Comment")]
    public class Comment
    {
        [DataMember]
        [XmlElement(ElementName = "SuggestionId")]
        public long? SuggestionId { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Question")]
        public string Question { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Answer")]
        public string Answer { get; set; }

        [DataMember]
        [XmlArray(ElementName = "Files")]
        public File[] Files { get; set; }

        [DataMember]
        [XmlElement(ElementName = "CreationDate")]
        public DateTime? CreationDate { get; set; }

        [DataMember]
        [XmlElement(ElementName = "QuestionDate")]
        public DateTime? QuestionDate { get; set; }

        [DataMember]
        [XmlElement(ElementName = "IsAcceptance")]
        public bool IsAcceptance { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Executor")]
        public string Executor { get; set; }
    }
}