namespace Bars.Gkh.Overhaul.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "GetPublishedProgrammResponse")]
    public class GetPublishedProgrammResponse
    {
        [DataMember]
        [XmlArray(ElementName = "PublishedProgrammRecords")]
        public GetPublishedProgrammRecord[] Records { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "PublishedProgrammRecord")]
    public class GetPublishedProgrammRecord
    {
        [DataMember]
        [XmlAttribute(AttributeName = "uid")]
        public virtual string Uid { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "okrug")]
        public virtual string Okrug { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "raion")]
        public virtual string Raion { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "address")]
        public virtual string Address { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "yearbuilding")]
        public virtual int BuildingYear { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "totalarea")]
        public virtual string TotalArea { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "es_period")]
        public virtual string EsPeriod { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "gas_period")]
        public virtual string GasPeriod { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "hvs_period")]
        public virtual string HvsPeriod { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "hvs_m_period")]
        public virtual string HvsmPeriod { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "gvs_period")]
        public virtual string GvsPeriod { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "gvs_m_period")]
        public virtual string GvsmPeriod { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "kan_period")]
        public virtual string KanPeriod { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "kan_m_period")]
        public virtual string KanmPeriod { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "otoplenie_period")]
        public virtual string OtopPeriod { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "otoplenie_m_period")]
        public virtual string OtopmPeriod { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "mus_period")]
        public virtual string MusPeriod { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ppiadu_period")]
        public virtual string PpiaduPeriod { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "pv_period")]
        public virtual string PvPeriod { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "fas_period")]
        public virtual string FasPeriod { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "krov_period")]
        public virtual string KrovPeriod { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "vdsk_period")]
        public virtual string VdskPeriod { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "lift_period")]
        public virtual string LiftPeriod { get; set; }
    }
}