namespace Bars.Gkh.Services.DataContracts.GetManOrgsByDate
{
    using Bars.Gkh.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts.GetMainInfoManOrg;
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "GetRoManOrgsResponseByDate")]
    public class GetRoManOrgsResponseByDate
    {
        [DataMember]
        [XmlArray(ElementName = "Information")]
        public ManOrgRealObject[] Information { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}