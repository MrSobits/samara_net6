using System.Collections;
using Bars.B4;
using Bars.GkhGji.Regions.Zabaykalye.DomainService;

namespace Bars.GkhGji.Regions.Zabaykalye.Controllers.ActCheck
{
    using Microsoft.AspNetCore.Mvc;

    public class ActCheckController : GkhGji.Controllers.ActCheckController
    {
        public ActionResult ListRealObjForActCheck(BaseParams baseParams)
        {
            var service = Container.Resolve<IZabaykalyeActCheckService>();

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