namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "InfoAboutReductPaymentItem")]
    public class InfoAboutReductPaymentItem
    {
        /// <summary>
        /// Идентификатор 
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// НаименованиеУслуги
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// ТипУслуги
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TypeGroupServiceDi")]
        public string TypeGroupServiceDi { get; set; }

        /// <summary>
        /// ПричинаСнижения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ReasonReduction")]
        public string ReasonReduction { get; set; }

        /// <summary>
        /// СуммаПерерасчета
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "RecalculationSum")]
        public string RecalculationSum { get; set; }

        /// <summary>
        /// ДатаПриказа
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "OrderDate")]
        public string OrderDate { get; set; }

        /// <summary>
        /// НомерПриказа
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "OrderNumber")]
        public string OrderNumber { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Description")]
        public string Description { get; set; }
    }
}