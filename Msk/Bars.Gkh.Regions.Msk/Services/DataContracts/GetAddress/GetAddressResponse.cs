namespace Bars.Gkh.Overhaul.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "GetAddressResponse")]
    public class GetAddressResponse
    {
        [DataMember]
        [XmlArray(ElementName = "AddressRecords")]
        public GetAddressRecord[] AddressRecords { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "AddressRecord")]
    public class GetAddressRecord
    {
        [DataMember]
        [XmlAttribute(AttributeName = "uid")]
        public virtual string Uid { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "address")]
        public virtual string Address { get; set; }
    }
}