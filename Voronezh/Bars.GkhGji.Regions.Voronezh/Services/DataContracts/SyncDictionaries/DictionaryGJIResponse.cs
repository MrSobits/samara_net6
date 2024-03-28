namespace Bars.GkhGji.Regions.Voronezh.Services.DataContracts.SyncDictionaries
{
    using Bars.Gkh.Services.DataContracts;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "DictionaryGJIResponse")]
    public class DictionaryGJIResponse
    {
        [DataMember]
        [XmlArray(ElementName = "DictionarysGJI")]
        public DictionaryGJI[] DictionarysGJI { get; set; }

        [DataMember]
        [XmlElement(ElementName = "RequestResult")]
        public RequestResult RequestResult { get; set; }
    }
}