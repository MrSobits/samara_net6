namespace Bars.Gkh.RegOperator.Services.DataContracts.GetCreditOpPayment
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Сведения об оплате КР
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "Payment")]
    public class Payment
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Дата начисления
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Начислено
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Credit")]
        public decimal Credit { get; set; }

        /// <summary>
        /// Оплачено
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Paid")]
        public decimal Paid { get; set; }

        /// <summary>
        /// Управляющая организация
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ManOrg")]
        public string ManOrg { get; set; }
    }
}