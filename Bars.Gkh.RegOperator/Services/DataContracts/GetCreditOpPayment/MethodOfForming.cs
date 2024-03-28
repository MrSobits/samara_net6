namespace Bars.Gkh.RegOperator.Services.DataContracts.GetCreditOpPayment
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Сведения об оплате КР
    /// </summary>
    [DataContract]
    [XmlType(TypeName = "MethodOfForming")]
    public class MethodOfForming
    {
        /// <summary>
        /// Способ формирования фонда КР
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "MethodOfFormingOverhaulFund")]
        public string MethodOfFormingOverhaulFund { get; set; }

        /// <summary>
        /// Номер протокола
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "CommonMeetingProtocolNumber")]
        public string CommonMeetingProtocolNumber { get; set; }

        /// <summary>
        /// Дата протокола
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "СommonMeetingProtocolDate")]
        public DateTime СommonMeetingProtocolDate { get; set; }
    }
}