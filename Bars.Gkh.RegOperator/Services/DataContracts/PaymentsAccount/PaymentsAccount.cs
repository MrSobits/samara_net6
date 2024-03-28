namespace Bars.Gkh.RegOperator.Services.DataContracts.PaymentsAccount
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "PaymentsAccount")]
    public class PaymentsAccount
    {
        /// <summary>
        /// Номер документа
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "NumDocument")]
        public string NumDocument { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DateDocument")]
        public string DateDocument { get; set; }

        /// <summary>
        /// Дата оплаты
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DatePay")]
        public string DatePay { get; set; }

        /// <summary>
        /// Тип оплаты
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TypePay")]
        public string TypePay { get; set; }

        /// <summary>
        /// Сумма платежа
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Sum")]
        public decimal Sum { get; set; }

        /// <summary>
        /// Пункт платежа
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "NamePA")]
        public string NamePa { get; set; }


        /// <summary>
        /// Пункт платежа
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PaymentState")]
        public string PaymentState { get; set; }
    }
}