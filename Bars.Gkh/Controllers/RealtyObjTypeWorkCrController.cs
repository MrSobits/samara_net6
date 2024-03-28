namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;

    using Bars.Gkh.DomainService;

    public class RealtyObjTypeWorkCrController : BaseController
    {
        public ActionResult List(BaseParams baseParams)
        {
            var listResult = (ListDataResult)Resolve<IRealtyObjectTypeWorkService>().List(baseParams);
            return new JsonNetResult(new { success = listResult.Success, data = listResult.Data, totalCount = listResult.TotalCount });
        }
    }
}
