namespace Bars.Gkh.RegOperator.Services.DataContracts.PaymentsAccount
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.Gkh.Services.DataContracts;

    /// <summary>
    /// Оплаты по лицеовму счету
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "PaymentsAccountResponse")]
    public class PaymentsAccountResponse
    {
        [DataMember]
        [XmlArray(ElementName = "PaymentsAccount")]
        public PaymentsAccount[] PaymentsAccount { get; set; }

        
        /// <summary>
        /// Result
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}