﻿namespace Bars.GkhGji.Regions.Chelyabinsk.Services.DataContracts.SyncInspections
{
    using Bars.Gkh.Services.DataContracts;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "DocumentsGJIData")]
    public class DocumentsGJIData
    {
        [DataMember]
        [XmlArray(ElementName = "Decisions")]
        public DecisionProxy[] Decisions { get; set; }

        [DataMember]
        [XmlArray(ElementName = "ActChecks")]
        public ActCheckProxy[] ActChecks { get; set; }

        [DataMember]
        [XmlArray(ElementName = "ActRemovals")]
        public ActRemovalProxy[] ActRemovals { get; set; }       

        [DataMember]
        [XmlArray(ElementName = "Prescriptions")]
        public PrescriptionProxy[] Prescriptions { get; set; }

        [DataMember]
        [XmlArray(ElementName = "Protocols")]
        public ProtocolProxy[] Protocols { get; set; }
    }
}