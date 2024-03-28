namespace Bars.Gkh.RegOperator.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Entities;

    public class PersonalAccountPeriodSummaryController : B4.Alt.DataController<PersonalAccountPeriodSummary>
    {
        public ActionResult GetAccountSummaryInfo(BaseParams baseParams)
        {
            var result = Resolve<IPersonalAccountSummaryService>().GetPeriodAccountSummary(baseParams);
            return JsSuccess(result.Data);
        }

        public ActionResult ListChargeParameterTrace(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IPersonalAccountSummaryService>().ListChargeParameterTrace(baseParams);
            return new JsonListResult((IList)result.Data);
        }

        public ActionResult ListPenaltyParameterTrace(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IPersonalAccountSummaryService>().ListPenaltyParameterTrace(baseParams);
            return new JsonListResult((IList)result.Data);
        }

        public ActionResult ListReCalcParameterTrace(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IPersonalAccountSummaryService>().ListReCalcParameterTrace(baseParams);
            return new JsonListResult((IList)result.Data);
        }

        public ActionResult ListRecalcPenaltyTrace(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IPersonalAccountSummaryService>().ListRecalcPenaltyTrace(baseParams);
            return new JsonListResult((IList)result.Data);
        }

        public ActionResult GetAccountSummaryInfoInCurrentPeriod(BaseParams baseParams)
        {
            var result = Resolve<IPersonalAccountSummaryService>().GetAccountSummaryInfoInCurrentPeriod(baseParams);
            return JsSuccess(result.Data);
        }

        public ActionResult ListRecalcPenaltyTraceDetail(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IPersonalAccountSummaryService>().ListRecalcPenaltyTraceDetail(baseParams);
            return new JsonListResult((IList)result.Data);
        }
    }
}