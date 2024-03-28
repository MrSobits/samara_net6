namespace Bars.Gkh.Services.DataContracts.GetHousesManOrgWithoutOpInf
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "HouseManOrgProxy")]
    public class HouseManOrgProxy
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Address")]
        public string Address { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AreaRooms")]
        public string AreaRooms { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "StartData")]
        public string StartData { get; set; }
    }
}