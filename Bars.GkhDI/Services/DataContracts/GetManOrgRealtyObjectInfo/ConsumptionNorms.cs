namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// НПА нормативов потребления
    /// </summary>
    public class ConsumptionNorms
    {
        /// <summary>
        /// (НПА нормативов потребления) Дата
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DocumentDate")]
        public DateTime DocumentDate { get; set; }

        /// <summary>
        /// (НПА нормативов потребления) Номер нормативно правового акта
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DocumentNumber")]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// (НПА нормативов потребления) Наименование принявшего акт органа
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DocumentOrganizationName")]
        public string DocumentOrganizationName { get; set; }
    }
}