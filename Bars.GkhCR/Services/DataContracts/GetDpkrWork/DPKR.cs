namespace Bars.GkhCr.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Информация о работах ДПКР
    /// </summary>
    public class DPKR
    {
        /// <summary>
        /// Id Версия программы
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "CommonEstateObjects")]
        public long Id { get; set; }

        /// <summary>
        /// Планируемые виды работ
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "CommonEstateObject")]
        public string CommonEstateObject { get; set; }

        /// <summary>
        /// Планированный год проведения ремонта
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Year")]
        public long Year { get; set; }

        /// <summary>
        /// Скорректированный год проведения ремонта
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "CorrectYear")]
        public long CorrectYear { get; set; }

        /// <summary>
        /// Сумма на работу
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Sum")]
        public decimal Sum { get; set; }
    }
}