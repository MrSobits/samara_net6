namespace Bars.Gkh.Overhaul.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.Gkh.Services.DataContracts;

    [DataContract]
    [XmlType(TypeName = "GetPublishProgramRecsResponse")]
    public class GetPublishProgramRecsResponse
    {
        [DataMember]
        [XmlElement(ElementName = "PublishProgRec")]
        public PublishProgRecWcfProxy[] PublishProgRecs { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}