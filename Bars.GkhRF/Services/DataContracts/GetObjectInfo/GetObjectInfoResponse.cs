namespace Bars.GkhRf.Services.DataContracts.GetObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "GetObjectInfoResponse")]
    public class GetObjectInfoResponse
    {
        [DataMember]
        [XmlElement(ElementName = "RealityObject")]
        public RealityObject RealityObject { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}