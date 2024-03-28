namespace Bars.Gkh.Services.DataContracts.GlonassIntegration
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Модель ответа сервиса на запрос информации по дому
    /// </summary>
    [DataContract]
    [XmlRoot(ElementName = "GetHouseResponse")]
    public class GetHouseResponse
    {
        /// <summary>Список домов</summary>
        [DataMember]
        [XmlElement(ElementName = "HousesInfo")]
        public HouseInfo HouseInfo { get; set; }

        /// <summary>Список домов</summary>
        [DataMember]
        [XmlArray(ElementName = "FlatsInfo")]
        public FlatInfo[] FlatsInfo { get; set; }

        /// <summary>Список домов</summary>
        [DataMember]
        [XmlArray(ElementName = "ProvidersInfo")]
        public ProviderInfo[] ProvidersInfo { get; set; }

        /// <summary>Список домов</summary>
        [DataMember]
        [XmlElement(ElementName = "UoInfo")]
        public UoInfo UoInfo { get; set; }

        /// <summary>Статус ответа сервиса</summary>
        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}