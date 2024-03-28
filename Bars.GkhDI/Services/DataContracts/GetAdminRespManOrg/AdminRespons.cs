namespace Bars.GkhDi.Services
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "AdministrativeResponsibility")]
    public class AdminRespons
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ControlOrg")]
        public string ControlOrg { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ViolCount")]
        public int ViolCount { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FineDate")]
        public string FineDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "SumFine")]
        public string SumFine { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DatePayFine")]
        public string DatePayFine { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Arrangements")]
        public string Arrangements { get; set; }
    }
}