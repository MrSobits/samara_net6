namespace Bars.Gkh.DeloIntegration.Wcf.Contracts
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "UpdateAnswerResponse")]
    public class UpdateAnswerResponse
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Success")]
        public bool Success { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Message")]
        public string Message { get; set; }
    }
}
