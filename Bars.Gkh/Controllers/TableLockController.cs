namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.TableLocker;

    public class TableLockController : BaseController
    {
        public ITableLockService Service { get; set; }

        public ActionResult List(BaseParams baseParams)
        {
            return Service.List(baseParams).ToJsonResult();
        }

        public ActionResult Unlock(BaseParams baseParams)
        {
            Service.Unlock(baseParams);
            return JsSuccess();
        }

        public ActionResult UnlockAll(BaseParams baseParams)
        {
            Service.UnlockAll();
            return JsSuccess();
        }
    }
}