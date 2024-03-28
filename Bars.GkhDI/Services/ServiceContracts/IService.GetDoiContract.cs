namespace Bars.GkhDi.Services
{
    using System.ServiceModel;

    using CoreWCF.Web;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "GetDoiContract/{roId},{periodId}")]
        GetDoiContractResponse GetDoiContract(string roId, string periodId);
    }
}
