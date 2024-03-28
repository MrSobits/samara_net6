namespace Bars.GkhGji.Regions.Voronezh.Services.DataContracts.SyncInspections
{
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Акт проверки исполнения предписания
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "ActRemovalProxy")]
    public class ActRemovalProxy
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }  

        /// <summary>
        /// Проверяемая площадь - берется из МКД
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Area")]
        public decimal? Area { get; set; }

        /// <summary>
        /// Место составления
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DocumentPlace")]
        public string DocumentPlace { get; set; }

        /// <summary>
        /// Дата и время составления акта
        /// </summary>me { get; set; }
        [DataMember]
        [XmlElement(ElementName = "DocumentDateTime")]
        public DateTime? DocumentDateTime { get; set; }

        /// <summary>
        /// Статус ознакомления с результатами проверки
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "AcquaintState")]
        public AcquaintState AcquaintState { get; set; }

        /// <summary>
        /// ФИО должностного лица, ознакомившегося/отказавшегося от ознакомления с актом проверки
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "AcquaintedPerson")]
        public string AcquaintedPerson { get; set; }

        /// <summary>
        /// Проверка проведена
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "IsEnded")]
        public bool IsEnded { get; set; }

        /// <summary>
        /// Нарушения устранены
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "IsRemooved")]
        public YesNoNotSet IsRemooved { get; set; }

        /// <summary>
        /// Нарушения
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "RealityObjectViolations")]
        public RealityObjectViolation[] RealityObjectViolations { get; set; }

        /// <summary>
        /// Лица, присутствующие при проверке (или свидетели)
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "ActCheckWitnessesproxy")]
        public ActCheckWitnessproxy[] ActCheckWitnessesproxy { get; set; }
    }
}