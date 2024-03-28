namespace Bars.GkhRf.Services.DataContracts.GetObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "ExpenditureCrItem")]
    public class ExpenditureCrItem
    {
        /// <summary>
        /// Дата
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "Date")]
        public string Date { get; set; }

        /// <summary>
        /// ПеречисленнаяСумма
        /// </summary>
        [DataMember]
        [XmlAttribute(AttributeName = "TransferSum")]
        public string TransferSum { get; set; }
    }
}