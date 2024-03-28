namespace Bars.GkhRf.Services.DataContracts.GetObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "ProgrammCrItem")]
    public class ProgrammCrItem
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// ДатаИзменения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DateChange")]
        public string DateChange { get; set; }

        /// <summary>
        /// ИдентификаторПрограммы
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "IdProg")]
        public long IdProg { get; set; }

        /// <summary>
        /// ГодФормированияПрограммы
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Year")]
        public string Year { get; set; }

        /// <summary>
        /// НаименованиеПрограммы
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "NameProg")]
        public string NameProg { get; set; }

        /// <summary>
        /// ЛимитПоПрограмме
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Limit")]
        public string Limit { get; set; }

        /// <summary>
        /// БюджетМО
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "BudgetMo")]
        public string BudgetMo { get; set; }

        /// <summary>
        /// БюджетСубъекта
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "BudgetRegion")]
        public string BudgetRegion { get; set; }

        /// <summary>
        /// СредстваСобственника
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ResourcesOwner")]
        public string ResourcesOwner { get; set; }

        /// <summary>
        /// СредстваФонда
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ResourcesFund")]
        public string ResourcesFund { get; set; }

        /// <summary>
        /// ПодрядныеОрганизации
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "ServiceOrg")]
        public ServiceOrgItem[] ServiceOrg { get; set; }

        /// <summary>
        /// СписокРабот
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "Work")]
        public WorkItem[] Work { get; set; }
    }
}