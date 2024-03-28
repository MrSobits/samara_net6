namespace Bars.GkhCr.Services
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.GkhCr.Services.DataContracts;

    [DataContract]
    [XmlType(TypeName = "GetHousesKrResponse")]
    public class GetHousesKrResponse
    {
        [DataMember]
        [XmlElement(ElementName = "ProgKr")]
        public ProgKr ProgKr { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}