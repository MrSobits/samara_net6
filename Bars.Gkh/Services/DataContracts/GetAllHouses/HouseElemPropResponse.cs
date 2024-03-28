namespace Bars.Gkh.Services.DataContracts.GetAllHouses
{
    using Bars.Gkh.Services.DataContracts;
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [Serializable]
    [XmlRoot(ElementName = "HouseElemPropResponse")]
    public class HouseElemPropResponse
    {
        [DataMember]
        [XmlArray(ElementName = "Houses")]
        public HouseProp[] Houses { get; set; }

        [DataMember]
        [XmlArray(ElementName = "Elements")]
        public HouseElements[] Elements { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}