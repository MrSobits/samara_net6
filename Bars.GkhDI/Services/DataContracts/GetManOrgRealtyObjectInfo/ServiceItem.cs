namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.GkhDi.Entities;

    [DataContract]
    [XmlType(TypeName = "ServiceItem")]
    public class ServiceItem
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
        /// Периодичность
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Periodicity")]
        public string Periodicity { get; set; }

        /// <summary>
        /// ПоставщикУслуги
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Provider")]
        public string Provider { get; set; }

        /// <summary>
        /// КодУслуги
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }

        /// <summary>
        /// ТипУслуги
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "KindService")]
        public string KindService { get; set; }

        /// <summary>
        /// ПоставляетсяЧерезУО_ТСЖ_ЖСК
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TypeProvisionUoTsjJsk")]
        public string TypeProvisionUoTsjJsk { get; set; }

        /// <summary>
        /// УслугаПредоставляется
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TypeProvision")]
        public string TypeProvision { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Price")]
        public string Price { get; set; }

        /// <summary>
        /// ОбъемЗакупаемыхРесурсов
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "VolumePurchasedResources")]
        public string VolumePurchasedResources { get; set; }

        /// <summary>
        /// ИсполнителемУслугиНеЯвляется
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "IsExecutor")]
        public string IsExecutor { get; set; }

        /// <summary>
        /// Тарифы РСО
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "TariffsForRso")]
        public TariffForRsoItem[] TariffsForRso { get; set; }

        /// <summary>
        /// Тарифы потребителей
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "TariffsForConsumers")]
        public TariffForConsumersItem[] TariffsForConsumers { get; set; }
    }
}