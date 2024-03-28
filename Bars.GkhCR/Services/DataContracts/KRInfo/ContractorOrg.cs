namespace Bars.GkhCr.Services.DataContracts.KRInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "ContractorOrg")]
    public class ContractorOrg
    {
        [DataMember]
        [XmlAttribute(AttributeName = "IdDoc")]
        public long IdDoc { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Contractor")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "LegalAddress")]
        public string LegalAddress { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "NameBoss")]
        public string NameBoss { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ContactBoss")]
        public string ContactBoss { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DocName")]
        public string DocName { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DocNum")]
        public string DocNum { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DateDoc")]
        public string DateDoc { get; set; }

        [DataMember]
        [XmlArray(ElementName = "ContractorWork")]
        public ContractorWork[] ContractorWork { get; set; }
    }
}