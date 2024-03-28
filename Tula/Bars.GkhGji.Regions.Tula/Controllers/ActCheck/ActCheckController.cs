using System.Collections;
using System.Web.Mvc;
using Bars.B4;
using Bars.GkhGji.Regions.Tula.DomainService;

namespace Bars.GkhGji.Regions.Tula.Controllers.ActCheck
{
    public class ActCheckController : GkhGji.Controllers.ActCheckController
    {
        public ActionResult ListRealObjForActCheck(BaseParams baseParams)
        {
            var service = Container.Resolve<ITulaActCheckService>();

            try
            {
                var result = (ListDataResult)service.ListRealObjForActCheck(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}