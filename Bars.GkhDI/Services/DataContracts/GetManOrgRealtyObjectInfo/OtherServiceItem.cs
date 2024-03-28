namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "OtherServiceItem")]
    public class OtherServiceItem
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// ЕдиницаИзмерения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "UnitMeasure")]
        public string UnitMeasure { get; set; }

        /// <summary>
        /// КодУслуги
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }

        /// <summary>
        /// Тарифы.
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "TariffsForConsumers")]
        public TariffForConsumersItem[] Tariffs { get; set; }
    }
}