namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Bars.Gkh.Services.DataContracts;

    [DataContract]
    [XmlRoot(ElementName = "GetReductionPaymentInfoResponse")]
    public class GetReductionPaymentInfoResponse
    {
        [DataMember]
        [XmlElement(ElementName = "HasReductionPayment")]
        public string HasReductionPayment { get; set; }

        [DataMember]
        [XmlArray(ElementName = "ReductionPayments")]
        public ReductionPayment[] ReductionPayments { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}