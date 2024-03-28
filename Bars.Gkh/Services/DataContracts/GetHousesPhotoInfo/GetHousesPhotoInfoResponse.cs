namespace Bars.Gkh.Services.DataContracts.GetHousesPhotoInfo
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "GetHousesPhotoInfoResponse")]
    public class GetHousesPhotoInfoResponse
    {
        [DataMember]
        [XmlArray(ElementName = "HousePhotos")]
        public HousePhoto[] HousePhotos { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}