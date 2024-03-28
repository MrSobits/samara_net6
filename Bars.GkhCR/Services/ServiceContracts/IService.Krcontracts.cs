namespace Bars.GkhCr.Services.ServiceContracts
{
    using System.ServiceModel;
    using CoreWCF.Web;

    using Bars.GkhCr.Services.DataContracts.KRInfo;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "KRContracts/{houseId}/{houseofRegoperator=null}")]
        KRInfoResponse KRContracts(string houseId, string houseofRegoperator);
    }
}