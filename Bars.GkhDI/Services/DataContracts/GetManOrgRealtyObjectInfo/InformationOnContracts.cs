namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "InformationOnContracts")]
    public class InformationOnContracts
    {
        /// <summary>
        /// ОбщееКоличество
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Count")]
        public int Count { get; set; }

        /// <summary>
        /// Договора
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "InformationOnCont")]
        public InformationOnContItem[] InformationOnCont { get; set; }
    }
}