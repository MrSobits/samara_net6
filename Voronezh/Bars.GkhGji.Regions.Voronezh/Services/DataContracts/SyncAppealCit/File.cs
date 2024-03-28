namespace Bars.GkhGji.Regions.Voronezh.Services.DataContracts.SyncAppealCit
{
    using Bars.Gkh.Enums;
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "File")]
    public class File
    {
        [DataMember]
        [XmlElement(ElementName = "FileName")]
        public string FileName { get; set; }

        [DataMember]
        [XmlElement(ElementName = "TypeDocument")]
        public string TypeDocument { get; set; }

        [DataMember]
        [XmlElement(ElementName = "NumberDocument")]
        public string NumberDocument { get; set; }

        [DataMember]
        [XmlElement(ElementName = "DocumentDate")]
        public string DocumentDate { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Base64")]
        public string Base64 { get; set; }
    }
}
