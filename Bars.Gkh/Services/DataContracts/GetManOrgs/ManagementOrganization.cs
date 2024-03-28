namespace Bars.Gkh.Services.DataContracts.ManagementOrganizationSearch
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "ManagementOrganization")]
    public class ManagementOrganization
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ContractorName")]
        public string ContractorName { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "MunicipalUnion")]
        public string MunicipalUnion { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Address")]
        public string Address { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Status")]
        public string Status { get; set; }
    }
}