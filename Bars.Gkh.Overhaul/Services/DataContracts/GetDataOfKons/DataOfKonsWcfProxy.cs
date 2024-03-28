namespace Bars.Gkh.Overhaul.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "DataOfKons")]
    public class DataOfKonsWcfProxy
    {
        [DataMember]
        [XmlAttribute(AttributeName = "GroupKons")]
        public string GroupKons { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "NameOfKons")]
        public string NameOfKons { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DateLastDid")]
        public int DateLastDid { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Wear")]
        public decimal Wear { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "UnitMeasure")]
        public string UnitMeasure { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Value")]
        public decimal Value { get; set; }
    }
}