namespace Bars.GkhCr.Controllers
{
    using B4;

    using Bars.B4.IoC;
    using Bars.GkhCr.DomainService;
    
    using Bars.GkhCr.Entities;

    using Microsoft.AspNetCore.Mvc;

    public class SpecialFinanceSourceResourceController : B4.Alt.DataController<SpecialFinanceSourceResource>
    {
        public ActionResult AddFinSources(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ISpecialFinanceSourceResourceService>();

            using (this.Container.Using(service))
            {
                var result = service.AddFinSources(baseParams);
                return new JsonNetResult(result);
            }
        }
    }
}