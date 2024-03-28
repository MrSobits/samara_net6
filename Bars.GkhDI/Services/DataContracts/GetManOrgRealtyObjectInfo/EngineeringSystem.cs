namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Инженерные системы
    /// </summary>
    public class EngineeringSystem
    {
        /// <summary>
        /// Тип системы теплоснабжения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "HeatingType")]
        public string HeatingType { get; set; }

        /// <summary>
        /// Тип системы горячего водоснабжения 
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "HotWaterType")]
        public string HotWaterType { get; set; }

        /// <summary>
        /// Тип системы холодного водоснабжения 
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "ColdWaterType")]
        public string ColdWaterType { get; set; }

        /// <summary>
        /// Тип системы водоотведения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "SewerageType")]
        public string SewerageType { get; set; }

        /// <summary>
        /// Объем выгребных ям, куб.м
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "SewerageCesspoolsVolume")]
        public string SewerageCesspoolsVolume { get; set; }

        /// <summary>
        /// Тип системы газоснабжения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "GasType")]
        public string GasType { get; set; }

        /// <summary>
        /// Тип системы вентиляции
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "VentilationType")]
        public string VentilationType { get; set; }

        /// <summary>
        /// Тип системы пожаротушения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "FirefightingType")]
        public string FirefightingType { get; set; }

        /// <summary>
        /// Тип системы водостоков
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DrainageType")]
        public string DrainageType { get; set; }

        /// <summary>
        /// Тип системы электроснабжения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "TypePower")]
        public string TypePower { get; set; }

        /// <summary>
        /// Количество вводов в дом
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "TypePowerPoints")]
        public string TypePowerPoints { get; set; }
    }
}