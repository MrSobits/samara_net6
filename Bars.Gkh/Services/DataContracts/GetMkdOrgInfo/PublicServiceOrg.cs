namespace Bars.Gkh.Services.DataContracts.GetMkdOrgInfo
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "RSO")]
    public class PublicServiceOrg
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Shortname")]
        public string Shortname { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "LegalForm")]
        public string LegalForm { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "TimeZone")]
        public string TimeZone { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Factaddr")]
        public string FactAddr { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "OKFS")]
        public string Okfs { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ContractId")]
        public long ContractId { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Date")]
        public string Date { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Num")]
        public string Num { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "IdFile")]
        public long FileId { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "File")]
        public string File { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FileExt")]
        public string FileExt { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FileName")]
        public string FileName { get; set; }
    }
}