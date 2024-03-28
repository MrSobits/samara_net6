namespace Bars.Gkh.Gku.Regions.Kamchatka.Controllers
{
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.Utils;

    using Microsoft.AspNetCore.Mvc;

    public class GkuExternalLinksController : BaseController
    {
        public ActionResult GetUrl(BaseParams baseParams)
        {
            var code = baseParams.Params.Get("code", string.Empty);
            return new JsonNetResult(Container.Resolve<IConfigProvider>().GetConfig().AppSettings.GetAs<string>(code));
        }
    }
}