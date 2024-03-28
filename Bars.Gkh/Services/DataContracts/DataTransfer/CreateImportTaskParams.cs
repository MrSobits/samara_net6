namespace Bars.Gkh.Services.DataContracts.DataTransfer
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "CreateImportTaskParams")]
    public class CreateImportTaskParams : DataTransferParameters
    {
        [DataMember]
        [XmlElement(ElementName = "Stream")]
        public byte[] Data { get; set; }
    }
}