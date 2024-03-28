namespace Bars.Gkh.Services.DataContracts.GetEmergencyObjectList
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "GetEmergencyObjectListResponse")]
    public class GetEmergencyObjectListResponse
    {
        [DataMember]
        [XmlArray(ElementName = "EmergencyObjects")]
        public EmergencyObjectProxy[] EmergencyObjects { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}