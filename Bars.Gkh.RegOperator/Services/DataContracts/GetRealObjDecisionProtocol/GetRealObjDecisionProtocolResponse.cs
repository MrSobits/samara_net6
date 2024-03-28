namespace Bars.Gkh.RegOperator.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "RealObjDecisionProtocol")]
    public class RealObjDecisionProtocolProxy
    {
        [DataMember]
        [XmlAttribute(AttributeName = "ProtocolType")]
        public string ProtocolType { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ProtocolDate")]
        public string ProtocolDate { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DateStart")]
        public string DateStart { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ProtocolNum")]
        public string ProtocolNum { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FileName")]
        public string FileName { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FileId")]
        public long FileId { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FundFormationType")]
        public string FundFormationType { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "SpecialAccOwner")]
        public string SpecialAccOwner { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "CreditOrg")]
        public string CreditOrg { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "MinFundAmount")]
        public string MinFundAmount { get; set; }
    }


    [DataContract]
    [XmlType(TypeName = "GetRealObjDecisionProtocolResponse")]
    public class GetRealObjDecisionProtocolResponse
    {
        [DataMember]
        [XmlArray(ElementName = "RealObjDecisionProtocols")]
        public RealObjDecisionProtocolProxy[] RealObjDecisionProtocols { get; set; }
    }
}