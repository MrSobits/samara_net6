namespace Bars.Gkh.Services.DataContracts.EmergencyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "EmergencyObjectInfo")]
    public class EmergencyObjectInfoProxy
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Address")]
        public string Address { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ResettlementProgram")]
        public string ResettlementProgram { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ActualityDate")]
        public string ActualityDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "IsRepairExpedient")]
        public string IsRepairExpedient { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ResettlementFlatArea")]
        public string ResettlementFlatArea { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ResettlementFlatAmount")]
        public string ResettlementFlatAmount { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "LandArea")]
        public string LandArea { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "CadastralNumber")]
        public string CadastralNumber { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DemolitionDate")]
        public string DemolitionDate { get; set; }
    }
}