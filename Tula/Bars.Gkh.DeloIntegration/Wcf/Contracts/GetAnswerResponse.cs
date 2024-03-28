namespace Bars.Gkh.DeloIntegration.Wcf.Contracts
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "GetAnswerResponse")]
    public class GetAnswerResponse
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Success")]
        public virtual bool Success { get; set; }

        [DataMember]
        [XmlArray(ElementName = "AnswerRecords")]
        public AnswerRecord[] Records { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Message")]
        public virtual string Message { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "AnswerRecord")]
    public class AnswerRecord
    {
        [DataMember]
        [XmlAttribute(AttributeName = "ExtId")]
        public virtual string AppealNumber { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AnswerId")]
        public virtual long AnswerId { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AnswerNum")]
        public virtual string AnswerNumber { get; set; }

        public long FileId { get; set; }

         /// <summary> File </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "File")]
        public byte[] File { get; set; }

        /// <summary> Имя файла (вместе с расширением) </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "FileName")]
        public string FileName { get; set; }
    }
}
