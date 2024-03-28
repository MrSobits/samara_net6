namespace Bars.Gkh.Services.DataContracts.GetManOrgsByDate
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "ManOrgRealObject")]
    public class ManOrgRealObject
    {
        [DataMember]
        [XmlAttribute(AttributeName = "RealObj")]
        public long RealObj { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ManOrg")]
        public long ManOrg { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Date")]
        public string Date { get; set; }
    }
}