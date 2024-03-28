namespace Bars.GkhCr.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.GkhCr.Services.DataContracts;

    [DataContract]
    [XmlType(TypeName = "GetProgKrResponse")]
    public class GetProgKrResponse
    {
        [DataMember]
        [XmlElement(ElementName = "ProgKr")]
        public ProgKr[] ProgKrs { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}