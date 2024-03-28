namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using System;

    public class HouseOverhaul
    {
        /// <summary>
        /// Наименование владельца специального счета
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "ProviderName")]
        public string ProviderName { get; set; }

        /// <summary>
        /// Дата протокола общего собрания собственников помещений, на котором принято решение о способе формирования фонда капитального ремонта
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "CommonMeetingProtocolDate")]
        public DateTime CommonMeetingProtocolDate { get; set; }

        /// <summary>
        /// Номер протокола общего собрания собственников помещений, на котором принято решение о способе формирования фонда капитального ремонта
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "CommonMeetingProtocolNumber")]
        public string CommonMeetingProtocolNumber { get; set; }

        /// <summary>
        /// Размер взноса на капитальный 
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PaymentAmountForms")]
        public decimal PaymentAmountForms { get; set; }
    }
}