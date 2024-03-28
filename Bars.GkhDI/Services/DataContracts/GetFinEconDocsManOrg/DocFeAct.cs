namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "DocFEAct")]
    public class DocFeAct
    {
        [DataMember]
        [XmlElement(ElementName = "AccReportThisYear")]
        public DocumentProxy AccReportThisYear { get; set; }

        [DataMember]
        [XmlElement(ElementName = "InExPastYear")]
        public DocumentProxy InExPastYear { get; set; }

        [DataMember]
        [XmlElement(ElementName = "FinBalance")]
        public DocumentProxy FinBalance { get; set; }

        [DataMember]
        [XmlElement(ElementName = "AuditPastYear")]
        public DocumentProxy AuditPastYear { get; set; }

        [DataMember]
        [XmlElement(ElementName = "AuditTwoYearsAgo")]
        public DocumentProxy AuditTwoYearsAgo { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ReportInExPastYear")]
        public DocumentProxy ReportInExPastYear { get; set; }

        [DataMember]
        [XmlElement(ElementName = "AuditThisYear")]
        public DocumentProxy AuditThisYear { get; set; }

        [DataMember]
        [XmlElement(ElementName = "InExThisYear")]
        public DocumentProxy InExThisYear { get; set; }

        [DataMember]
        [XmlElement(ElementName = "AccReportTwoYearsAgo")]
        public DocumentProxy AccReportTwoYearsAgo { get; set; }

        [DataMember]
        [XmlElement(ElementName = "AccReportPastYear")]
        public DocumentProxy AccReportPastYear { get; set; }

        [DataMember]
        [XmlElement(ElementName = "AppAccBalance")]
        public DocumentProxy AppAccBalance { get; set; }
    }
}