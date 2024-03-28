namespace Bars.GkhDi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Entities;

    public class PlanReductionExpenseController : B4.Alt.DataController<PlanReductionExpense>
    {
        public ActionResult AddBaseService(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IPlanReductionExpenseService>().AddBaseService(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}

