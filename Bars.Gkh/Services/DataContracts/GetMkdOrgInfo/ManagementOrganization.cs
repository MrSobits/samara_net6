namespace Bars.Gkh.Services.DataContracts.GetMkdOrgInfo
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "UO")]
    public class ManagementOrganization
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
        [XmlAttribute(AttributeName = "DUid")]
        public long Duid { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DateDU")]
        public string DateDu { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "NumDU")]
        public string NumDu { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FileDUid")]
        public long FileDuid { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FileDU")]
        public string FileDu { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FileExt")]
        public string FileExt { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FileName")]
        public string FileName { get; set; }
    }
}