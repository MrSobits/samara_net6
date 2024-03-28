namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "PlanReductionExpItem")]
    public class PlanReductionExpItem
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// СрокВыполнения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DateComplete")]
        public string DateComplete { get; set; }

        /// <summary>
        /// ПредСниженРас
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PlannedReductionExpense")]
        public string PlannedReductionExpense { get; set; }

        /// <summary>
        /// ФактСниженРас
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "FactedReductionExpense")]
        public string FactedReductionExpense { get; set; }

        /// <summary>
        /// ПричинаОтклонения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ReasonRejection")]
        public string ReasonRejection { get; set; }
    }
}