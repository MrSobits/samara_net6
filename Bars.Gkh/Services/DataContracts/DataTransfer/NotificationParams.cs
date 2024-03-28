namespace Bars.Gkh.Services.DataContracts.DataTransfer
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.Gkh.SystemDataTransfer.Enums;

    [DataContract]
    [XmlRoot(ElementName = "NotificationParams")]
    public class NotificationParams : DataTransferParameters
    {
        [DataMember]
        [XmlElement(ElementName = "OperationType")]
        public DataTransferOperationType OperationType { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Success")]
        public bool Success { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Message")]
        public string Message { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Finished")]
        public bool IsFinised { get; set; }
    }
}