namespace Bars.GkhCr.Services.ServiceContracts
{
    using System.ServiceModel;
    using CoreWCF.Web;

    using Bars.GkhCr.Services.DataContracts.KRInfo;

    public partial interface IService
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebGet(UriTemplate = "KRInfo/{houseId}/{houseofRegoperator=null}")]
        KRInfoResponse KRInfo(string houseId, string houseofRegoperator);
    }
}