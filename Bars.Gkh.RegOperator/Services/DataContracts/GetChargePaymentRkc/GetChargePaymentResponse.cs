using System;

namespace Bars.Gkh.RegOperator.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [Serializable]
    [XmlType(TypeName = "GetChargePaymentResponse")]
    public class GetChargePaymentResponse
    {
        [DataMember]
        [XmlElement(ElementName = "FILE")]
        public GetChargePaymentRecord Record { get; set; }
        
        [DataMember]
        [XmlElement(ElementName = "RESULT")]
        public GetChargePaymentResult Result { get; set; }
    }

    [DataContract]
    [Serializable]
    [XmlRoot(ElementName = "FILE")]
    public class GetChargePaymentRecord
    {
        [DataMember]
        [XmlAttribute("FORMAT_VERSION")]
        public string FormatVersion { get; set; }

        [DataMember]
        [XmlAttribute("REG_CHARGE_SUM")]
        public string RegChargeSum { get; set; }

        [DataMember]
        [XmlAttribute("REG_PAID_SUM")]
        public string RegPaidSum { get; set; }

        [DataMember]
        [XmlAttribute("PAGE")]
        public string Page { get; set; }

        [DataMember]
        [XmlAttribute("COUNT")]
        public string Count { get; set; }

        [DataMember]
        [XmlArray("PAYMENTS")]
        [XmlArrayItem("PAYMENT", typeof(SyncChargePayment))]
        public SyncChargePayment[] Payments { get; set; }
    }
}