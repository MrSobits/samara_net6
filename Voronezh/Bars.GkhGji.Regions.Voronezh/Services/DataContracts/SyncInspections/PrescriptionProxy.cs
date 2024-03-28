namespace Bars.GkhGji.Regions.Voronezh.Services.DataContracts.SyncInspections
{
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;


    [DataContract]
    [XmlType(TypeName = "PrescriptionProxy")]
    public class PrescriptionProxy
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
        /// Контрагент
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ContragentProxy")]
        public ContragentProxy ContragentProxy { get; set; }

        /// <summary>
        /// Физическое лицо
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PhysicalPerson")]
        public string PhysicalPerson { get; set; }

        /// <summary>
        /// Реквизиты физ. лица
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PhysicalPersonInfo")]
        public string PhysicalPersonInfo { get; set; }

        /// <summary>
        /// Нарушения
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "RealityObjectViolations")]
        public RealityObjectViolation[] RealityObjectViolations { get; set; }
    }

    

}