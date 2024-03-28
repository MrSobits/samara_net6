using System.Collections;
using System.Web.Mvc;
using Bars.B4;
using Bars.B4.Modules.Security;
using Bars.Gkh.UserActionRetention.DomainService;

namespace Bars.Gkh.UserActionRetention.Controllers
{
    public class UserLoginController : B4.Alt.DataController<User>
    {
        public IUserLoginService AuditLogMapService { get; set; }

        public ActionResult ListWithoutPaging(BaseParams baseParams)
        {
            var result = (ListDataResult)AuditLogMapService.ListWithoutPaging(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }
    }
}
