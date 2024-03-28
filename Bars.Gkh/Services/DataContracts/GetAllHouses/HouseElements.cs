namespace Bars.Gkh.Services.DataContracts.GetAllHouses
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "HouseElements")]
    public class HouseElements
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "RealityObject")]
        public int RealityObject { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "GroupName")]
        public string GroupName { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ElementName")]
        public string ElementName { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "YearRepair")]
        public int YearRepair { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Wear")]
        public decimal Wear { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Volume")]
        public decimal Volume { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Unit")]
        public string Unit { get; set; }
    }
}