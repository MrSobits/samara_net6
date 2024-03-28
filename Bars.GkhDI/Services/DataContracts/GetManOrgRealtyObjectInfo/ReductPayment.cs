namespace Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "ReductPayment")]
    public class ReductPayment
    {
        /// <summary>
        /// СлучайСниженияПлаты
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "InfoAboutReductPayment")]
        public InfoAboutReductPaymentItem[] InfoAboutReductPayment { get; set; }

        /// <summary>
        /// СлучаевСниженияПлатыНеБыло
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "IsReductionPayment")]
        public string IsReductionPayment { get; set; }
    }
}