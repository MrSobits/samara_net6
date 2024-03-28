namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "TariffForRso")]
    public class TariffForRsoItem
    {
        /// <summary>
        /// Дата начала действия
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DateStart")]
        public string DateStart { get; set; }

        /// <summary>
        /// Дата окончания действия
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TariffEndDate")]
        public string DateEnd { get; set; }

        /// <summary>
        /// Номер нормативно правового акта
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "NumberNormativeLegalAct")]
        public string NumberNormativeLegalAct { get; set; }

        /// <summary>
        /// Дата нормативно правового акта
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DateNormativeLegalAct")]
        public string DateNormativeLegalAct { get; set; }

        /// <summary>
        /// Орган, установивший тариф
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "OrganizationSetTariff")]
        public string OrganizationSetTariff { get; set; }

        /// <summary>
        /// Стоимость тарифа
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Cost")]
        public string Cost { get; set; }
    }
}