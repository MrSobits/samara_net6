using System.Collections;
using System.Web.Mvc;
using Bars.B4;
using Bars.Gkh.UserActionRetention.DomainService;

namespace Bars.Gkh.UserActionRetention.Controllers
{
    public class AuditLogMapController : BaseController
    {
        public IAuditLogMapService AuditLogMapService { get; set; }

        public ActionResult List(BaseParams baseParams)
        {
            var result = (ListDataResult)AuditLogMapService.List(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        public ActionResult ListWithoutPaging(BaseParams baseParams)
        {
            var result = (ListDataResult)AuditLogMapService.ListWithoutPaging(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }
    }
}
