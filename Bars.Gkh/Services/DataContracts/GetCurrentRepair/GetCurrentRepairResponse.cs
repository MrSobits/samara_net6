namespace Bars.Gkh.Services.DataContracts.CurrentRepair
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "GetCurrentRepairResponse")]
    public class GetCurrentRepairResponse
    {
        [DataMember]
        [XmlArray(ElementName = "CurrentRepairs")]
        public CurrentRepair[] CurrentRepairs { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}