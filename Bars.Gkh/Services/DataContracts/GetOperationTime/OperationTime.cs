namespace Bars.Gkh.Services.DataContracts.GetOperationTime
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "OperationTime")]
    public class OperationTime
    {
        [DataMember]
        [XmlArray(ElementName = "ModeOfOrganizations")]
        [XmlArrayItem(ElementName = "ModeOfOrganization")]
        public WorkingHours[] ModeOfOrganizations { get; set; }

        [DataMember]
        [XmlArray(ElementName = "CitizenRecHours")]
        [XmlArrayItem(ElementName = "CitizenRecHour")]
        public WorkingHours[] CitizenRecHours { get; set; }

        [DataMember]
        [XmlArray(ElementName = "DispatcherTimes")]
        [XmlArrayItem(ElementName = "DispatcherTime")]
        public WorkingHours[] DispatcherTimes { get; set; }
    }
}