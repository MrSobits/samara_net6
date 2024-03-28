namespace Bars.GkhCr.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Hmao.DomainService;
    using Bars.GkhCr.Entities;

    public class FinanceSourceResourceController : B4.Alt.DataController<FinanceSourceResource>
    {
        public ActionResult AddFinSources(BaseParams baseParams)
        {
            var service = Container.Resolve<IFinanceSourceResourceService>();

            try
            {
                var result = service.AddFinSources(baseParams);
                return new JsonNetResult(result);
            }
            finally
            {
                Container.Release(service);
            }
        }

    }
}