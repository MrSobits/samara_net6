namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "Documents")]
    public class Documents
    {
        /// <summary>
        /// ПротоколыОбщихСобранийТекущегоГода
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DocumentCurrentYear")]
        public DocumentRealObj DocumentCurrentYear { get; set; }

        /// <summary>
        /// ПротоколыОбщихСобранийПредыдущегоГода
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DocumentPrevYear")]
        public DocumentRealObj DocumentPrevYear { get; set; }

        /// <summary>
        /// ОтчетыОВыполненииГодовогоПлана
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "DocumentReportOfPlan")]
        public DocumentRealObj DocumentReportOfPlan { get; set; }
    }
}