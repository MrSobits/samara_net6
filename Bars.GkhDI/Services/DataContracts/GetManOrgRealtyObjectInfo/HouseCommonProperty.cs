namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Сведения об использовании мест общего пользования
    /// </summary>
    public class HouseCommonProperty
    {
        /// <summary>
        /// Договоры на использование мест общего пользования имеются
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "HCP")]
        public string Hcp { get; set; }

        /// <summary>
        /// Наименование общего имущества
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "NameHCP")]
        public string NameHcp { get; set; }

        /// <summary>
        /// Назначение общего имущества
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "FunctionHCP")]
        public string FunctionHcp { get; set; }

        /// <summary>
        /// Площадь общего имущества
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "AreaHCP")]
        public decimal AreaHcp { get; set; }
    }
}