namespace Bars.GkhRf.Services.DataContracts.GetRegOpPayment
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "GetRegOpPaymentResponse")]
    public class GetRegOpPaymentResponse
    {
        [DataMember]
        [XmlArray(ElementName = "Payments")]
        public Payment[] Payments { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}