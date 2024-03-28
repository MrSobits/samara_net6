namespace Bars.Gkh.Services.DataContracts.Suggestion
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    
    [DataContract]
    [XmlRoot(ElementName = "Suggestion")]
    public class Suggestion
    {
        [DataMember]
        [XmlElement(ElementName = "Id")]
        public long? Id { get; set; }

        [DataMember]
        [XmlElement(ElementName = "StateId")]
        public long? StateId { get; set; }

        [DataMember]
        [XmlElement(ElementName = "StateName")]
        public string StateName { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Number")]
        public string Number { get; set; }

        [DataMember]
        [XmlElement(ElementName = "RealityObjectId")]
        public long? RealityObjectId { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Rubric")]
        public Rubric Rubric { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ApplicantFio")]
        public string ApplicantFio { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ApplicantAddress")]
        public string ApplicantAddress { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ApplicantPhone")]
        public string ApplicantPhone { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ApplicantEmail")]
        public string ApplicantEmail { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ProblemPlace")]
        public ProblemPlace ProblemPlace { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }

        [DataMember]
        [XmlElement(ElementName = "HasAnswer")]
        public bool? HasAnswer { get; set; }

        [DataMember]
        [XmlElement(ElementName = "AnswerText")]
        public string AnswerText { get; set; }

        [DataMember]
        [XmlElement(ElementName = "AnswerDate")]
        public DateTime? AnswerDate { get; set; }

        [DataMember]
        [XmlArray(ElementName = "Files")]
        public File[] Files { get; set; }

        [DataMember]
        [XmlArray(ElementName = "Comments")]
        public Comment[] Comments { get; set; }

        [DataMember]
        [XmlElement(ElementName = "MessageId")]
        public string MessageId { get; set; }

        [DataMember]
        [XmlElement(ElementName = "CategoryId")]
        public string CategoryId { get; set; }

        [DataMember]
        [XmlElement(ElementName = "FlatId")]
        public string FlatId { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Executor")]
        public string Executor { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Deadline")]
        public DateTime? Deadline { get; set; }

        [DataMember]
        [XmlElement(ElementName = "FirstExecutor")]
        public string FirstExecutor { get; set; }

        [DataMember]
        [XmlElement(ElementName = "DeadLineDays")]
        public int DeadLineDays { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Test")]
        public bool? Test { get; set; }
    }
}