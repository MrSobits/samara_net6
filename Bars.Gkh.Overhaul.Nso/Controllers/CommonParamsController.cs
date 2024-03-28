namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using B4;
    using Overhaul.CommonParams;

    public class CommonParamsController: BaseController
    {
        public virtual ActionResult List(BaseParams baseParams)
        {
            var commonParams =
                Container.ResolveAll<ICommonParam>()
                    .Select(x => new
                    {
                        x.Name,
                        x.Code,
                        x.CommonParamType
                    });

            return JsSuccess(commonParams);
        }
    }
}