namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using B4;

    using Entities;
    using DomainService;

    public class SupplyResourceOrgRealtyObjectController : B4.Alt.DataController<SupplyResourceOrgRealtyObject>
    {
        public ActionResult AddRealtyObjects(BaseParams baseParams)
        {
            var result = Container.Resolve<ISupplyResourceOrgRealtyObjectService>().AddRealtyObjects(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }
    }
}