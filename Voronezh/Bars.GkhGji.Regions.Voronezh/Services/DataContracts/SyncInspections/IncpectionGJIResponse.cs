namespace Bars.GkhGji.Regions.Voronezh.Services.DataContracts.SyncInspections
{
    using Bars.Gkh.Services.DataContracts;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "IncpectionGJIResponse")]
    public class IncpectionGJIResponse
    {
        [DataMember]
        [XmlArray(ElementName = "IncpectionsGJI")]
        public IncpectionGJIProxy[] IncpectionsGJI { get; set; }

        [DataMember]
        [XmlElement(ElementName = "DocumentsGJIData")]
        public DocumentsGJIData DocumentsGJIData { get; set; }

        [DataMember]
        [XmlElement(ElementName = "RequestResult")]
        public RequestResult RequestResult { get; set; }
    }

    [DataContract]
    [XmlRoot(ElementName = "IncpectionGJIRequest")]
    public class IncpectionGJIRequest
    {
        [DataMember]
        [XmlArray(ElementName = "IncpectionsGJI")]
        public IncpectionGJIProxy[] IncpectionsGJI { get; set; }

        [DataMember]
        [XmlElement(ElementName = "DocumentsGJIData")]
        public DocumentsGJIData DocumentsGJIData { get; set; }
    }

}