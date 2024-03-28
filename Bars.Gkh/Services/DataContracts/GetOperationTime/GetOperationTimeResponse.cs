namespace Bars.Gkh.Services.DataContracts.GetOperationTime
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "GetOperationTimeResponse")]
    public class GetOperationTimeResponse
    {
        [DataMember]
        [XmlElement(ElementName = "OperationTime")]
        public OperationTime OperationTime { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}