namespace Bars.Gkh.Services.DataContracts.GetAllHouses
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "AllHouses")]
    public class AllHouses
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "NameMO")]
        public string NameMO { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FiasAddress")]
        public string FiasAddress { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "HouseGuid")]
        public string HouseGuid { get; set; }
    }
}