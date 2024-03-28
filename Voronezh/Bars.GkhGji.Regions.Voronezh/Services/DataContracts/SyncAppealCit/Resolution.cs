namespace Bars.GkhGji.Regions.Voronezh.Services.DataContracts.SyncAppealCit
{
    using Bars.Gkh.Enums;
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "Resolution")]
    public class Resolution
    {
        [DataMember]
        [XmlElement(ElementName = "ResolutionText")]
        public string ResolutionText { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ResolutionTerm")]
        public string ResolutionTerm { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ResolutionAuthor")]
        public string ResolutionAuthor { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ResolutionDate")]
        public string ResolutionDate { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ResolutionContent")]
        public string ResolutionContent { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ImportId")]
        public string ImportId { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ParentId")]
        public string ParentId { get; set; }

        [DataMember]
        [XmlArray(ElementName = "ResolutionExecutors")]
        public ResolutionExecutor[] ResolutionsExecutors { get; set; }
    }
}
