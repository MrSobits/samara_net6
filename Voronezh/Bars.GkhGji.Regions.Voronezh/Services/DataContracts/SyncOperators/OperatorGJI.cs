namespace Bars.GkhGji.Regions.Voronezh.Services.DataContracts.SyncOperators
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "OperatorGJI")]
    public class OperatorGJI
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Login")]
        public string Login { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Password")]
        public string Password { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "IsGJI")]
        public bool IsGJI { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Position")]
        public string Position { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ObjectEditDate")]
        public DateTime ObjectEditDate { get; set; }
    }
}