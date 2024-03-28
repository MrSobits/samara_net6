namespace Bars.GkhRf.Services.DataContracts.GetObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "TransfersRegFundItem")]
    public class TransfersRegFundItem
    {
        /// <summary>
        /// ПеречисленнаяСумма
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "SumTransfer")]
        public string SumTransfer { get; set; }

        /// <summary>
        /// МесяцПеречислений
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DateRegFund")]
        public string DateRegFund { get; set; }

        /// <summary>
        /// ДатаПоручения
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "DateDecree")]
        public string DateDecree { get; set; }
    }
}