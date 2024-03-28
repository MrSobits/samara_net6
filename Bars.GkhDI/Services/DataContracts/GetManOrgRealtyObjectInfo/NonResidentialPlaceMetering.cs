namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "NonResidentialPlaceMetering")]
    public class NonResidentialPlaceMetering
    {
        /// <summary>
        /// Наименование
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// КлассТочности
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "AccuracyClass")]
        public string AccuracyClass { get; set; }

        /// <summary>
        /// ТипУчета
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TypeAccounting")]
        public string TypeAccounting { get; set; }

    }
}