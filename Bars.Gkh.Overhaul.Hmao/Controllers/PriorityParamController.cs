namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;

    public class PriorityParamController : BaseController
    {
        public IPriorityParamService Service { get; set; }

        public ActionResult List(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.List(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }
    }
}