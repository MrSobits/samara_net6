namespace Bars.Gkh.Overhaul.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.Gkh.Services.DataContracts;

    [DataContract]
    [XmlType(TypeName = "GetRepairKonstResponse")]
    public class GetRepairKonstResponse
    {
        [DataMember]
        [XmlArray(ElementName = "GetRepairKonst")]
        public RepairKonstProxy[] RepairKonstProxy { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}