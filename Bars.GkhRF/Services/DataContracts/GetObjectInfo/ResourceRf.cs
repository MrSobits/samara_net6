namespace Bars.GkhRf.Services.DataContracts.GetObjectInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "ResourceRf")]
    public class ResourceRf
    {
        /// <summary>
        /// ДенежныеСредстваРегФонд
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "TransfersRegFund")]
        public TransfersRegFundItem[] TransfersRegFund { get; set; }

        /// <summary>
        /// РасходКапРемонт
        /// </summary>
        [DataMember]
        [XmlArray(ElementName = "ExpenditureCr")]
        public ExpenditureCrItem[] ExpenditureCr { get; set; }
    }
}