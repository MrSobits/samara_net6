namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Overhaul.CommonParams;

    public class CommonParamsController: BaseController
    {
        public virtual ActionResult List(BaseParams baseParams)
        {
            var commonParams = Container.ResolveAll<ICommonParam>()
                .ToList<ICommonParam>()
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