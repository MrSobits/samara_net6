namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.DomainService;
    using DomainService.Interface;
    
    public class StatusPaymentDocumentHouses : BaseController
    {
        public ActionResult GetStatusPaymentDocumentHouses(BaseParams baseParams)
        {
            var result = this.Resolve<IStatusPaymentDocumentHousesService>().GetStatusPaymentDocumentHouses(baseParams);
            return result.ToJsonResult();
          //  return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}