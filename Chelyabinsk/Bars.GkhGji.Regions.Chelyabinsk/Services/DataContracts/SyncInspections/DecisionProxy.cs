﻿namespace Bars.GkhGji.Regions.Chelyabinsk.Services.DataContracts.SyncInspections
{
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

  
    [DataContract]
    [XmlType(TypeName = "DecisionProxy")]
    public class DecisionProxy
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "TypeDisposalGji")]
        public TypeDisposalGji TypeDisposalGji { get; set; }

        [DataMember]
        [XmlElement(ElementName = "DateStart")]
        public DateTime? DateStart { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "TypeAgreementProsecutor")]
        public TypeAgreementProsecutor TypeAgreementProsecutor { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "TypeAgreementResult")]
        public TypeAgreementResult TypeAgreementResult { get; set; }

        [DataMember]
        [XmlElement(ElementName = "IssuedDisposal")]
        public InspectorProxy IssuedDisposal { get; set; }

        /// <summary>
        /// Вид проверки код справочника 4
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "KindCheckGjiId")]
        public long KindCheckGjiId { get; set; }

        [DataMember]
        [XmlElement(ElementName = "DocumentDate")]
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Выезд на объект с
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ObjectVisitStart")]
        public DateTime? ObjectVisitStart { get; set; }

        /// <summary>
        /// Выезд на объект по
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ObjectVisitEnd")]
        public DateTime? ObjectVisitEnd { get; set; }

        /// <summary>
        /// Цели проверки
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "AgreementResult")]
        public string AgreementResult { get; set; }

        /// <summary>
        /// Основание решения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "BaseText")]
        public string BaseText { get; set; }

        /// <summary>
        /// Проверяющие
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "Inspectors")]
        public InspectorProxy[] Inspectors { get; set; }
        
        /// <summary>
        /// Период проверки
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PeriodCorrect")]
        public string PeriodCorrect { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "KindKNDGJI")]
        public KindKNDGJI KindKNDGJI;

    }

}