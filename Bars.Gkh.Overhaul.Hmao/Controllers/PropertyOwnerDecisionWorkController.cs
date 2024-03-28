namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class PropertyOwnerDecisionWorkController : B4.Alt.DataController<PropertyOwnerDecisionWork>
    {
        public ActionResult AddWorks(BaseParams baseParams)
        {
            var result = (BaseDataResult)Container.Resolve<IPropertyOwnerDecisionWorkService>().AddWorks(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }
    }
}