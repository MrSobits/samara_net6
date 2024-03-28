namespace Bars.Gkh.Gis.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Bars.Gkh.Gis.DomainService.Analysis;
    using Castle.Windsor;
    using DomainService.Indicator;
    using DomainService.RealEstate;

    internal class RegressionAnalysisController : BaseController
    {
        protected IRegressionAnalysisService RegressionAnalysisService;

        public RegressionAnalysisController(
            IRegressionAnalysisService regressionAnalysisService)
        {
            RegressionAnalysisService = regressionAnalysisService;
        }

        public ActionResult GetHouseTypes(BaseParams baseParams)
        {
            var result = RegressionAnalysisService.GroupedTypeWithoutEmptyGroupList(baseParams);
            return new JsonNetResult(new {success = true, children = result.Data});
        }

        public ActionResult GetIndicatorsTypes(BaseParams baseParams)
        {
            var result = (ListDataResult) RegressionAnalysisService.IndicatorsRegressionAnalysis(baseParams);
            return new JsonNetResult(new {success = true, children = result.Data});
        }


        public ActionResult GetChartData(BaseParams baseParams)
        {
            var result = RegressionAnalysisService.ChartRegressionAnalysis(baseParams) as ListDataResult;
            return new JsonNetResult(new {success = true, data = result != null ? result.Data : null});
        }
    }
}
