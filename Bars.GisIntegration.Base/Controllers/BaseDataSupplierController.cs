namespace Bars.GisIntegration.Base.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Service;
    using Bars.GisIntegration.Base.Service.Impl;

    public abstract class BaseDataSupplierController : BaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var id = filterContext.HttpContext.Request.Params["dataSupplierId"].ToLong();
            if (id > 0)
            {
                new DataSupplierContext(id);
            }

            base.OnActionExecuting(filterContext);
        }

        protected override void Dispose(bool disposing)
        {
            if (DataSupplierContext.Current != null)
            {
                DataSupplierContext.Current.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}