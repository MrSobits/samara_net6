namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "UseCommonFacil")]
    public class UseCommonFacil
    {
        /// <summary>
        /// ДоговорыНеЗаключены
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "PlaceGeneralUse")]
        public string PlaceGeneralUse { get; set; }

        /// <summary>
        /// МестаОбщегоПользования
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "InfoAboutUseCommonFacil")]
        public InfoAboutUseCommonFacilItem[] InfoAboutUseCommonFacil { get; set; }
    }
}