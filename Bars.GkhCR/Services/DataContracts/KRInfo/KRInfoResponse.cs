namespace Bars.GkhCr.Services.DataContracts.KRInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Bars.GkhCr.Services.DataContracts;

    [DataContract]
    [XmlRoot(ElementName = "KRInfoResponse")]
    public class KRInfoResponse
    {
        [DataMember]
        [XmlArray(ElementName = "ProgramsKR")]
        public ProgramKR[] ProgramsKR { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}