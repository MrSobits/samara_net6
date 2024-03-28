namespace Bars.GkhRf.Services.DataContracts.GetObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "MovementMoneyItem")]
    public class MovementMoneyItem
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// КодДвиженияДенежныхСредств
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }

        /// <summary>
        /// МесяцНачисления
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Date")]
        public string Date { get; set; }

        /// <summary>
        /// ВходящееСальдоНачисления
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ImpBalance")]
        public string ImpBalance { get; set; }

        /// <summary>
        /// ИсходящееСальдоНачисления
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ExpBalance")]
        public string ExpBalance { get; set; }

        /// <summary>
        /// ПерерасчетПрошлогоПериода
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Recalculation")]
        public string Recalculation { get; set; }

        /// <summary>
        /// НачисленоНаселению
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Accrual")]
        public string Accrual { get; set; }

        /// <summary>
        /// ОплаченоНаселением
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Payed")]
        public string Payed { get; set; }

        /// <summary>
        /// ОбщаяПлощадь
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "GeneralArea")]
        public string GeneralArea { get; set; }

        /// <summary>
        /// УправляющаяКомпания
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ManOrg")]
        public string ManOrg { get; set; }
    }
}