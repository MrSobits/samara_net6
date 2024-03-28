namespace Bars.GkhGji.Regions.Voronezh.Services.DataContracts.SyncInspections
{
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "IncpectionGJIProxy")]
    public class IncpectionGJIProxy
    {
        /// <summary>
        /// Ид проверки
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "TypeJurPerson")]
        public TypeJurPerson TypeJurPerson { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ContragentProxy")]
        public ContragentProxy ContragentProxy { get; set; }

        /// <summary>
        /// Тип основания проверки
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TypeBase")]
        public TypeBase TypeBase { get; set; }

        /// <summary>
        /// даттые основания (номер и дата обращения, распоряжения и тд)
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "BaseDocAttr")]
        public string BaseDocAttr { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "InspectionNumber")]
        public string InspectionNumber { get; set; }

        /// <summary>
        /// Контрольный срок
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "CheckDate")]
        public DateTime CheckDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PhysicalPerson")]
        public string PhysicalPerson { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PhysicalPersonInfo")]
        public string PhysicalPersonInfo { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "KindKNDGJI")]
        public KindKNDGJI KindKNDGJI { get; set; }

        [DataMember]
        [XmlArray(ElementName = "RealityObjects")]
        public IncpectionGJIRealityObject[] RealityObjects { get; set; }

        [DataMember]
        [XmlArray(ElementName = "DocumentsGJI")]
        public DocumentGJIProxy[] DocumentsGJI { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "IncpectionGJIRealityObject")]
    public class IncpectionGJIRealityObject
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "RoId")]
        public long RoId { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Address")]
        public string Address { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "MunicipalityName")]
        public string MunicipalityName { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Area")]
        public decimal? Area { get; set; }

    }

    [DataContract]
    [XmlType(TypeName = "DocumentGJIProxy")]
    public class DocumentGJIProxy
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Ид родительского документа
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ParentId")]
        public long? ParentId { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "TypeDocumentGji")]
        public TypeDocumentGji TypeDocumentGji { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DocumentNumber")]
        public string DocumentNumber { get; set; }

        [DataMember]
        [XmlElement(ElementName = "DocumentDate")]
        public DateTime? DocumentDate { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "InspectorProxy")]
    public class InspectorProxy
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FIO")]
        public string FIO { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Position")]
        public string Position { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "ContragentProxy")]
    public class ContragentProxy
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Inn")]
        public string Inn { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Ogrn")]
        public string Ogrn { get; set; }
    }
}