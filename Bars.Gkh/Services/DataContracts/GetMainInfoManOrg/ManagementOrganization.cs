namespace Bars.Gkh.Services.DataContracts.GetMainInfoManOrg
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    [DataContract]
    [XmlType(TypeName = "ManagementOrganization", AnonymousType = true)]
    public class ManagementOrganization
    {
        [IgnoreDataMember]
        [XmlIgnore]
        public Contragent Contragent { get; set; }

        [IgnoreDataMember]
        [XmlIgnore]
        public TypeManagementManOrg TypeManagement { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ManOrgName")]
        public string ManOrgName { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ManForm")]
        public string ManForm { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "BossName")]
        public string BossName { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "BossContact")]
        public string BossContact { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "LegalAddress")]
        public string LegalAddress { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "RealAddress")]
        public string RealAddress { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PostAddress")]
        public string PostAddress { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "OfficalSite")]
        public string OfficalSite { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Email")]
        public string Email { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Twitter")]
        public string Twitter { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FrguRegNumber")]
        public string FrguRegNumber { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FrguOrgNumber")]
        public string FrguOrgNumber { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FrguServiceNumber")]
        public string FrguServiceNumber { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "INN")]
        public string INN { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "OGRN")]
        public string OGRN { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "RegistOrg")]
        public string RegistOrg { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Dispatcher")]
        public string Dispatcher { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ContactNumber")]
        public string ContactNumber { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DirectorFio")]
        public string DirectorFio { get; set; }

        [DataMember]
        [XmlArray(ElementName = "BoardMembers")]
        public BoardMember[] BoardMembers { get; set; }

        [DataMember]
        [XmlArray(ElementName = "CommitMembers")]
        public CommitMember[] CommitMembers { get; set; }

        [DataMember]
        [XmlArray(ElementName = "CommitNumbers")]
        public Phone[] CommitNumbers { get; set; }

        [DataMember]
        [XmlArray(ElementName = "AssocMemberships")]
        public AssocMembership[] AssocMemberships { get; set; }
    }
}