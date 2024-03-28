namespace Bars.Gkh.Decisions.Nso.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Domain;
    using Entities;

    public class MonthlyFeeAmountDecisionController : B4.Alt.BaseDataController<MonthlyFeeAmountDecision>
    {
        public ActionResult SaveHistory(BaseParams baseParams)
        {
            var result = (BaseDataResult)Container.Resolve<IMonthlyFeeAmountDecisionService>().SaveHistory(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}