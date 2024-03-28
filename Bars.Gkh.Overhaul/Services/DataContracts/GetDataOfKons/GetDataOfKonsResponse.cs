namespace Bars.Gkh.Overhaul.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.Gkh.Services.DataContracts;

    [DataContract]
    [XmlType(TypeName = "GetDataOfKonsResponse")]
    public class GetDataOfKonsResponse
    {
        [DataMember]
        [XmlArray(ElementName = "GetDataOfKons")]
        public DataOfKonsWcfProxy[] DataOfKons { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}