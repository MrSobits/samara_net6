namespace Bars.GkhCr.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.GkhCr.Services.DataContracts;

    /// <summary>
    /// Ответ на добавление отчета
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "InsertHousekeeperReportResponce")]
    public class InsertHousekeeperReportResponce
    {
        /// <summary>
        /// Результат 
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }

    /// <summary>
    /// Ответ на добавление отчета
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "GetHousekeeperReportResponce")]
    public class GetHousekeeperReportResponce
    {
        /// <summary>
        /// Результат 
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "HousekeeperReports")]
        public HousekeeperReportProxy[] HousekeeperReports { get; set; }

        /// <summary>
        /// Результат 
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}