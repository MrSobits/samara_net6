namespace Bars.Gkh.DeloIntegration.Wcf.Contracts
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "Result")]
    public class DeloResult
    {
        /// <summary> Номер обращения в системе </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ExtId")]
        public string ExtId { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Success")]
        public bool Success { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Message")]
        public string Message { get; set; }
    }
}
