namespace Bars.Gkh.Services.DataContracts.DataTransfer
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "DataTransferParameters")]
    public class DataTransferParameters
    {
        [DataMember]
        [XmlElement(ElementName = "Guid")]
        public Guid Guid { get; set; }
    }
}