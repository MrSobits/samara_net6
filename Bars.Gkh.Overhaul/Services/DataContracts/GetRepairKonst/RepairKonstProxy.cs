namespace Bars.Gkh.Overhaul.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "RepairKonstProxy")]
    public class RepairKonstProxy
    {
        [DataMember]
        [XmlAttribute(AttributeName = "NameOoi")]
        public string NameOoi { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "YearPublic")]
        public int YearPublic { get; set; }
    }
}