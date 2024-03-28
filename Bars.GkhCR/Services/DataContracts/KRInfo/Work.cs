namespace Bars.GkhCr.Services.DataContracts.KRInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "Work")]
    public class Work
    {
        [DataMember]
        [XmlAttribute(AttributeName = "IdWork")]
        public long IdWork { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Measure")]
        public string Measure { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PlanYear")]
        public string PlanYear { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PlanSum")]
        public decimal PlanSum { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PlanVolume")]
        public decimal PlanVolume { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "StartDate")]
        public string StartDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FinishDate")]
        public string FinishDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Percent")]
        public decimal Percent { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Sum")]
        public decimal Sum { get; set; }

        [DataMember]
        [XmlArray(ElementName = "PhotoArchive")]
        public Photo[] PhotoArchive { get; set; }

        [DataMember]
        [XmlArray(ElementName = "WorkActs")]
        public WorkAct[] WorkActs { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Volume")]
        public decimal Volume { get; set; }

        [DataMember]
        [XmlArray(ElementName = "Contractors")]
        public Contractor[] Contractors { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FactVolume")]
        public decimal FactVolume { get; set; }
    }
}