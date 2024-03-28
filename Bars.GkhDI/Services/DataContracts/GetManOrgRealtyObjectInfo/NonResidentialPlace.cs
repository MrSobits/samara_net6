namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "NonResidentialPlace")]
    public class NonResidentialPlace
    {
        /// <summary>
        /// ДанныеОбИспользованииНежилыхПомещенийОтсутствуют
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "NonResidentialPlacement")]
        public string NonResidentialPlacement { get; set; }

        /// <summary>
        /// Нежилые помещения
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "NonResidentialPlaceItem")]
        public NonResidentialPlaceItem[] NonResidentialPlaceItem { get; set; }
    }
}