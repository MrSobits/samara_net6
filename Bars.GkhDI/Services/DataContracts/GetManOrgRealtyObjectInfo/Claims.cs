namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Информация о наличии претензий по качеству выполненных работ (оказанных услуг)
    /// </summary>
    public class Claims
    {
        /// <summary>
        /// Количество поступивших претензий
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "СlaimsReceivedCount")]
        public int СlaimsReceivedCount { get; set; }

        /// <summary>
        /// Количество удовлетворенных претензий
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ClaimsSatisfiedCount")]
        public int ClaimsSatisfiedCount { get; set; }

        /// <summary>
        /// Количество претензий по качеству
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ClaimsDeniedCount")]
        public int ClaimsDeniedCount { get; set; }

        /// <summary>
        /// Сумма произведенного перерасчета
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ProducedRecalculationAmount")]
        public decimal ProducedRecalculationAmount { get; set; }
    }
}