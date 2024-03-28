namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.Config;

    public class BillingController : BaseController
    {
        public ActionResult GetUrl()
        {
            return new JsonNetResult(Container.Resolve<IConfigProvider>().GetConfig().AppSettings.GetAs<string>("Overhaul_BillingUrl"));
        }
    }
}
