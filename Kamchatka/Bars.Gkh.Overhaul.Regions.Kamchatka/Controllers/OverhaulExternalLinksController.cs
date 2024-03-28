namespace Bars.Gkh.Overhaul.Regions.Kamchatka.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.Utils;

    public class OverhaulExternalLinksController : BaseController
    {
        public ActionResult GetUrl(BaseParams baseParams)
        {
            var code = baseParams.Params.Get("code", string.Empty);
            return new JsonNetResult(Container.Resolve<IConfigProvider>().GetConfig().AppSettings.GetAs<string>(code));
        }
    }
}