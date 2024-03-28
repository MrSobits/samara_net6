namespace Bars.GkhGji.Regions.Voronezh.Services.DataContracts.SyncInspections
{
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

  
    [DataContract]
    [XmlType(TypeName = "ActCheckProxy")]
    public class ActCheckProxy
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
        /// Лицо ознакомленное/отказавшееся ознакомиться с результатами проверки
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
        /// Выявлены нарушения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "HasViolations")]
        public YesNoNotSet HasViolations { get; set; }

        /// <summary>
        /// ФИО должностного лица, ознакомившегося/отказавшегося от ознакомления с актом проверки
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
        
        /// <summary>
        /// Сведения о лицах, допустивших нарушения
        /// </summary>
        /// 
        [DataMember]
        [XmlArray(ElementName = "PersonViolationInfo")]
        public byte[] PersonViolationInfo { get; set; }

        /// <summary>
        /// Сведения о том, что нарушения были допущены в результате
        /// виновных действий (бездействия) должностных лиц и/или
        /// работников проверяемого лица
        /// (о ужос)
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "PersonViolationActionInfo")]
        public virtual byte[] PersonViolationActionInfo { get; set; }

        /// <summary>
        /// Описание нарушения
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "ViolationDescription")]
        public virtual byte[] ViolationDescription { get; set; }
    }

    /// <summary>
    /// Нарушения выявленные на объекте проверки
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "RealityObjectViolation")]
    public class RealityObjectViolation
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Дом из проверки
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "IncpectionGJIRealityObject")]
        public IncpectionGJIRealityObject IncpectionGJIRealityObject { get; set; }

        /// <summary>
        /// ид нарушения из справочника 2 Нарушения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ViolationId")]
        public long ViolationId { get; set; }

        /// <summary>
        /// Плановая дата устранения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DatePlanRemoval")]
        public DateTime? DatePlanRemoval { get; set; }

        /// <summary>
        /// фактическая дата устранения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DateFactRemoval")]
        public DateTime? DateFactRemoval { get; set; }

        /// <summary>
        /// продленная дата устранения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DatePlanExtension")]
        public DateTime? DatePlanExtension { get; set; }
        
        
        /// <summary>
        /// Описание нарушения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ViolationDescription")]
        public string ViolationDescription { get; set; }
        

        [DataMember]
        [XmlAttribute(AttributeName = "Action")]
        public string Action { get; set; }
    
    }

    /// <summary>
    /// Лица, присутствующие при проверке (или свидетели)
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "ActCheckWitnessProxy")]
    public class ActCheckWitnessproxy
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Position")]
        public string Position { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Fio")]
        public string Fio { get; set; }

        /// <summary>
        /// С актом ознакомлен
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "IsFamiliar")]
        public bool IsFamiliar { get; set; }
    }


}