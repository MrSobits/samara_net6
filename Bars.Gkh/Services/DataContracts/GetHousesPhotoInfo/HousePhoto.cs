namespace Bars.Gkh.Services.DataContracts.GetHousesPhotoInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "HousePhoto")]
    public class HousePhoto
    {
        [DataMember]
        [XmlAttribute(AttributeName = "HouseId")]
        public long Id { get; set; }

        [DataMember]
        [XmlArray(ElementName = "Photos")]
        public Photo[] Photos { get; set; }
    }
}