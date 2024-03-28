namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.Config;

    public class BillingController : BaseController
    {
        public IConfigProvider ConfigProvider { get; set; }

        public ActionResult GetUrl()
        {
            return new JsonNetResult(ConfigProvider.GetConfig().AppSettings.GetAs<string>("Overhaul_BillingUrl"));
        }
    }
}
