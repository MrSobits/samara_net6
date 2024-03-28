namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// Лифты в доме
    /// </summary>
    public class HouseLift
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// Номер подъезда
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PorchNumber")]
        public string PorchNumber { get; set; }

        /// <summary>
        /// Идентификатор типа лифта 
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }

        /// <summary>
        /// Год ввода в эксплуатацию
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "CommissioningYear")]
        public string CommissioningYear { get; set; }
    }
}