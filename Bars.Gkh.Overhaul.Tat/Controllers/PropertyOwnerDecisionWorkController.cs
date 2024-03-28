namespace Bars.Gkh.Overhaul.Tat.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Tat.DomainService;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class PropertyOwnerDecisionWorkController : B4.Alt.DataController<PropertyOwnerDecisionWork>
    {
        public ActionResult AddWorks(BaseParams baseParams)
        {
            var result = (BaseDataResult)Container.Resolve<IPropertyOwnerDecisionWorkService>().AddWorks(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        public ActionResult PropertyOwnerDecisionTypeList(BaseParams baseParams)
        {
            var result = (ListDataResult)Container.Resolve<IPropertyOwnerDecisionWorkService>().PropertyOwnerDecisionTypeList(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult MethodFormFundCrTypeList(BaseParams baseParams)
        {
            var result = (ListDataResult)Container.Resolve<IPropertyOwnerDecisionWorkService>().MethodFormFundCrTypeList(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }
    }
}