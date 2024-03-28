namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "TariffForConsumers")]
    public class TariffForConsumersItem
    {
        /// <summary>
        /// Дата начала действия
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DateStart")]
        public string DateStart { get; set; }

        /// <summary>
        /// Дата начала действия
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TariffEndDate")]
        public string DateEnd { get; set; }

        /// <summary>
        /// Тариф установлен для
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TariffIsSetFor")]
        public string TariffIsSetFor { get; set; }

        /// <summary>
        /// Орган, установивший тариф
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "OrganizationSetTariff")]
        public string OrganizationSetTariff { get; set; }

        /// <summary>
        /// Тип организации установившей тариф
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TypeOrganSetTariffDi")]
        public string TypeOrganSetTariffDi { get; set; }

        /// <summary>
        /// Стоимость тарифа
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Cost")]
        public string Cost { get; set; }
    }
}