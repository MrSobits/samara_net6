namespace Bars.Gkh.RegOperator.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "Charge")]
    public class ChargeProxy
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Sum")]
        public string Sum { get; set; }
    }
}