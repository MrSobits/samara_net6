namespace Bars.Gkh1468.Controllers.Passport
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;

    public class HousePassportCombinedController : BaseController
    {
        public ActionResult GetList(BaseParams baseParams)
        {
            var result = Resolve<IHousePassportCombinedService>().GetList(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message, result.Data);
        }
    }
}