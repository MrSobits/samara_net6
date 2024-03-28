namespace Bars.Gkh.RegOperator.Services.DataContracts.GetCreditOpPayment
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Сведения об оплате КР
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "TotalPayments")]
    public class TotalPayments
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Начислено всего
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TotalCredit")]
        public decimal TotalCredit { get; set; }

        /// <summary>
        /// Оплачено всего
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TotalPaid")]
        public decimal TotalPaid { get; set; }
    }
}