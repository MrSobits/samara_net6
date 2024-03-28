namespace Bars.GkhGji.Regions.Voronezh.Services.DataContracts.SyncOperators
{
    using Bars.Gkh.Services.DataContracts;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "OperatorGJIResponse")]
    public class OperatorGJIResponse
    {
        [DataMember]
        [XmlArray(ElementName = "OperatorsGJI")]
        public OperatorGJI[] OperatorsGJI { get; set; }

        [DataMember]
        [XmlElement(ElementName = "RequestResult")]
        public RequestResult RequestResult { get; set; }
    }
}