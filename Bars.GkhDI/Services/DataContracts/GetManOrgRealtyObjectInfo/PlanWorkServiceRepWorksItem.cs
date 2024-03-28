namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "DetailPpr")]
    public class DetailPpr
    {
        /// <summary>
        /// Наименование работы в детализации
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Запланированный объем 
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PlanSize")]
        public decimal PlanSize { get; set; }

        /// <summary>
        /// Фактический объем
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "FactSize")]
        public decimal FactSize { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Izm")]
        public string Izm { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "WorkPpr", Namespace = "ManOrg")]
    public class WorkPpr
    {
        /// <summary>
        /// Наименование
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "NamePpr")]
        public string NamePpr { get; set; }

        /// <summary>
        /// Периодичность
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PeriodicityTemplateService")]
        public string PeriodicityTemplateService { get; set; }

        /// <summary>
        /// Сведения о выполнении
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Information")]
        public string Information { get; set; }

        /// <summary>
        /// Дата начала  и Дата окончания  работ
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DateComplete")]
        public string DateComplete { get; set; }

        /// <summary>
        /// Дата окончания работ
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DateEnd")]
        public string DateEnd { get; set; }

        /// <summary>
        /// Стоимость
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Cost")]
        public decimal Cost { get; set; }

        /// <summary>
        /// Фактическая стоимость
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "FactCost")]
        public decimal FactCost { get; set; }

        /// <summary>
        /// Причина отклонения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ReasonRejection")]
        public string ReasonRejection { get; set; }

        /// <summary>
        /// Детализация по ППР
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "Details")]
        public DetailPpr[] Details { get; set; }

    }

    //
    [DataContract]
    [XmlType(TypeName = "DetailTo")]
    public class DetailTo
    {
        /// <summary>
        /// Наименование работы в детализации
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "WorkTo", Namespace = "ManOrg")]
    public class WorkTo
    {
        /// <summary>
        /// Наименование группы ТО
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "NameGroup")]
        public string NameGroup { get; set; }

        /// <summary>
        /// Дата начала  и Дата окончания  работ
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DateComplete")]
        public string DateComplete { get; set; }

        /// <summary>
        /// Дата окончания работ
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DateEnd")]
        public string DateEnd { get; set; }

        /// <summary>
        /// Причина отклонения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ReasonRejection")]
        public string ReasonRejection { get; set; }

        /// <summary>
        /// Детализация по ТО
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "DetailsTo")]
        public DetailTo[] DetailsTo { get; set; }
    }

    //

    [DataContract]
    [XmlType(TypeName = "PlanWorkServiceRepWorksItem")]
    public class PlanWorkServiceRepWorksItem
    {
        /// <summary>
        /// Работа по ППР
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "WorksPpr")]
        public WorkPpr[] WorksPpr { get; set; }

        /// <summary>
        /// Работа по ТО
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "WorksTo")]
        public WorkTo[] WorksTo { get; set; }
    }

}