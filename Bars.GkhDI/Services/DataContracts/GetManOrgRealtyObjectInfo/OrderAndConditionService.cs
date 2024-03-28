namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "OrderAndConditionService")]
    public class OrderAndConditionService
    {
        /// <summary>
        /// АктОСостоянииОбщегоИмущества
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Act")]
        public DocumentRealObj Act { get; set; }

        /// <summary>
        /// ПереченьОбязательныхИДополнительныхРабот
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "WorksList")]
        public DocumentRealObj WorksList { get; set; }

        /// <summary>
        /// Документы
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Documents")]
        public Documents Documents { get; set; }

        /// <summary>
        /// ПланРаботИМерПоСнижениюРасходов
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "PlanReductionExp")]
        public PlanReductionExpItem[] PlanReductionExp { get; set; }

        /// <summary>
        /// РаботыПоСодержаниюИРемонтуМКД
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "PlanWorkServiceRepWorks")]
        public PlanWorkServiceRepWorksItem PlanWorkServiceRepWorks { get; set; }

        /// <summary>
        /// СведенияОСлучаяхСниженияПлаты  
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ReductPayment")]
        public ReductPayment ReductPayment { get; set; }

        /// <summary>
        /// СведенияОДоговорах 
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "InformationOnContracts")]
        public InformationOnContracts InformationOnContracts { get; set; }
    }
}