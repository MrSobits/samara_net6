using Microsoft.AspNetCore.Mvc;
using Bars.B4;
using Bars.B4.Utils;
using Bars.GkhGji.Regions.Tatarstan.DomainService;

namespace Bars.GkhGji.Regions.Tatarstan.Controller
{
    public class GmpInnCheckerController : BaseController
    {
        public ActionResult Check(string inn, string kpp)
        {
            var config = Resolve<IGjiTatParamService>().GetConfig();

            return new JsonNetResult(new 
            {
                boolean = (string)config.Get("GisGmpPayeeInn") == inn && (string)config.Get("GisGmpPayeeKpp") == kpp
            });
        }
    }
}