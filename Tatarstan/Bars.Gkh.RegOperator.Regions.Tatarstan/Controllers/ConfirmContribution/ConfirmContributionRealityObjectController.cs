namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.DomainService;

    public class ConfirmContributionRealityObjectController : B4.Alt.DataController<RealityObject>
    {
        public ActionResult ListRealObj(BaseParams baseParams)
        {
            var service = Container.Resolve<IConfirmContributionService>();
            try
            {
                var result = (ListDataResult)service.RealObjList(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}