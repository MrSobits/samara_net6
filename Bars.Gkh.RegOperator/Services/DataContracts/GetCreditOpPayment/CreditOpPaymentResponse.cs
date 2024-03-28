namespace Bars.Gkh.RegOperator.Services.DataContracts.GetCreditOpPayment
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.Gkh.Services.DataContracts;

    /// <summary>
    /// Сведения об оплатах КР
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "CreditOpPaymentResponse")]
    public class CreditOpPaymentResponse
    {
        /// <summary>
        /// Payments
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "Payments")]
        public Payment[] Payments { get; set; }

        /// <summary>
        /// TotalPayments
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "TotalPayments")]
        public TotalPayments TotalPayments { get; set; }

        /// <summary>
        /// MethodOfForming
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "MethodOfForming")]
        public MethodOfForming MethodOfForming { get; set; }

        /// <summary>
        /// Result
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}