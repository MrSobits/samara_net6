namespace Bars.GkhCr.Services
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Информация о работах ДПКР
    /// </summary>
    public class HousekeeperReportProxy
    {
        /// <summary>
        /// Id Версия программы
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "HousekeeperLogin")]
        public string HousekeeperLogin { get; set; }

        /// <summary>
        /// Планируемые виды работ
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ObjectCR")]
        public long ObjectCR { get; set; }

        /// <summary>
        /// Планированный год проведения ремонта
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ReportDate")]
        public DateTime ReportDate { get; set; }

        /// <summary>
        /// Скорректированный год проведения ремонта
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ReportNumber")]
        public string ReportNumber { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "State")]
        public string State { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "CheckDate")]
        public string CheckDate { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "CheckTime")]
        public string CheckTime { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Answer")]
        public string Answer { get; set; }

        /// <summary>
        /// Устранено
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "IsArranged")]
        public bool IsArranged { get; set; }

        /// <summary>
        /// Файлы  отчета
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ReportFiles")]
        public ReportFile[] ReportFiles { get; set; }
    }
 

    [DataContract]
    [XmlRoot(ElementName = "ReportFile")]
    public class ReportFile
    {
        [DataMember]
        [XmlElement(ElementName = "FileName")]
        public string FileName { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }       

        [DataMember]
        [XmlElement(ElementName = "Base64")]
        public string Base64 { get; set; }
    }
}