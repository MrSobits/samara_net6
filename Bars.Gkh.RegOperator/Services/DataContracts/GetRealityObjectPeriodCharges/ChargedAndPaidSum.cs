namespace Bars.Gkh.RegOperator.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "ChargedAndPaidSum")]
    public class ChargedAndPaidSum
    {
        [DataMember]
        [XmlAttribute(AttributeName = "FlatNum")]
        public string FlatNum { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ChargedSum")]
        public decimal ChargedSum { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PaidSum")]
        public decimal PaidSum { get; set; }

        [DataMember]
        [XmlElement(ElementName = "PersonalAccountNum")]
        public string PersonalAccountNum { get; set; }
    }
}