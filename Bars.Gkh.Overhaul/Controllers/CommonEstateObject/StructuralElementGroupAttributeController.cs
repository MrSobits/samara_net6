namespace Bars.Gkh.Overhaul.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using DomainService;
    using Entities;
    using Gkh.Entities.CommonEstateObject;

    public class StructuralElementGroupAttributeController : B4.Alt.DataController<StructuralElementGroupAttribute>
    {
        public ActionResult ListWithResolvers(BaseParams baseParams)
        {
            var service = Container.Resolve<IStructuralElementGroupAttributeService>();
            return new JsonNetResult(service.ListWithResolvers(baseParams));
        }
    }
}