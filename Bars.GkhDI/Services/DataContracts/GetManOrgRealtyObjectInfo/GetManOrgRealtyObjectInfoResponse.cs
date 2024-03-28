namespace Bars.GkhDi.Services.DataContracts.GetPeriods
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.Gkh.Services.DataContracts;
    using Bars.GkhDi.Services.DataContracts.GetManOrgRealtyObjectInfo;

    [DataContract]
    [XmlRoot(ElementName = "GetManOrgRealtyObjectInfoResponse")]
    public class GetManOrgRealtyObjectInfoResponse
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "RealityObjectItem")]
        public RealityObjectItem RealityObjectItem { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }

    }
}