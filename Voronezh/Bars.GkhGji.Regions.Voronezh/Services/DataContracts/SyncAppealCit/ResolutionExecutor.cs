namespace Bars.GkhGji.Regions.Voronezh.Services.DataContracts.SyncAppealCit
{
    using Bars.Gkh.Enums;
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "ResolutionExecutor")]
    public class ResolutionExecutor
    {
        [DataMember]
        [XmlElement(ElementName = "Surname")]
        public string Surname { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Patronymic")]
        public string Patronymic { get; set; }

        [DataMember]
        [XmlElement(ElementName = "PersonalTerm")]
        public string PersonalTerm { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Comment")]
        public string Comment { get; set; }

        [DataMember]
        [XmlElement(ElementName = "IsResponsible")]
        public bool IsResponsible { get; set; }
    }
}
