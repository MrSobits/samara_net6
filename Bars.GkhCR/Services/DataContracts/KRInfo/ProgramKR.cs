namespace Bars.GkhCr.Services.DataContracts.KRInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "ProgramKR")]
    public class ProgramKR
    {
        [DataMember]
        [XmlAttribute(AttributeName = "IdProg")]
        public long IdProg { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "NameProg")]
        public string NameProg { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Year")]
        public int Year { get; set; }

        [DataMember]
        [XmlArray(ElementName = "Works")]
        public Work[] Works { get; set; }

        [DataMember]
        [XmlArray(ElementName = "ContractorOrgs")]
        public ContractorOrg[] ContractorOrgs { get; set; }

        [DataMember]
        [XmlArray(ElementName = "FundingSources")]
        public FundingSources[] FundingSources { get; set; }
    }
}