namespace Bars.Gkh.Regions.Msk.Services
{
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using Bars.Gkh.Overhaul.Services.DataContracts;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetPublishedProgramm")]
        GetPublishedProgrammResponse GetPublishedProgramm();
    }
}