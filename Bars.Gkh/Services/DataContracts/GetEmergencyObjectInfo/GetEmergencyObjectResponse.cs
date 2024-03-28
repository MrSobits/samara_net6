namespace Bars.Gkh.Services.DataContracts.EmergencyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "GetEmergencyObjectInfoResponse")]
    public class GetEmergencyObjectInfoResponse
    {
        [DataMember]
        [XmlArray(ElementName = "EmergencyObjectInfo")]
        public EmergencyObjectInfoProxy[] EmergencyObjectInfo { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}