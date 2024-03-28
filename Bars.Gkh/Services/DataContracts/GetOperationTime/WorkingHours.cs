namespace Bars.Gkh.Services.DataContracts.GetOperationTime
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class WorkingHours
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Day")]
        public string Day { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "OfficeHours")]
        public string OfficeHours { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Break")]
        public string Break { get; set; }
    }
}