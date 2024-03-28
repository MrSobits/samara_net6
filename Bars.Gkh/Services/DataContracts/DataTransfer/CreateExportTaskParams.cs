namespace Bars.Gkh.Services.DataContracts.DataTransfer
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "CreateExportTaskParams")]
    public class CreateExportTaskParams : DataTransferParameters
    {
        [DataMember]
        [XmlArray("TypeName")]
        public string[] TypeNames { get; set; }

        [DataMember]
        [XmlElement(ElementName = "ExportDependencies")]
        public bool ExportDependencies { get; set; }
    }
}