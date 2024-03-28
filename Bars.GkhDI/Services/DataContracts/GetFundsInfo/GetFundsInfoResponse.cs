namespace Bars.GkhDi.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Bars.Gkh.Services.DataContracts;

    [DataContract]
    [XmlRoot(ElementName = "GetFundsInfoResponse")]
    public class GetFundsInfoResponse
    {
        [DataMember]
        [XmlArray(ElementName = "Funds")]
        public Fund[] Funds { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}