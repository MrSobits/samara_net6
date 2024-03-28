namespace Bars.Gkh1468.Controllers.Passport
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Bars.Gkh1468.Entities;


    public class OkiPassportCombinedController : BaseController
    {
        public ActionResult GetList(BaseParams baseParams)
        {
            var result = Resolve<IOkiPassportCombinedService>().GetList(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message, result.Data);
        }
    }
}