using Bars.GkhGji.Regions.Habarovsk.Services.DataContracts.SyncAppealCitFromEDM;
using System.ServiceModel;

namespace Bars.GkhGji.Regions.Habarovsk.Services.ServiceContracts
{
    using CoreWCF.Web;

    [ServiceContract]
    public interface IAppealCitFromEDMService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string GetAppealCitFromEDM(RegCardToBarsDto request);
    }
}
