namespace Bars.GkhDi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Entities;

    public class FinActivityManagRealityObjController : B4.Alt.DataController<FinActivityManagRealityObj>
    {
        public ActionResult SaveManagRealityObj(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IFinActivityManagRealityObjService>().SaveManagRealityObj(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}
