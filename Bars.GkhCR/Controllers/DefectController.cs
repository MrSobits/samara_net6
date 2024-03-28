namespace Bars.GkhCr.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;
    using B4;

    using Bars.Gkh.ConfigSections.Cr;
    using Bars.Gkh.ConfigSections.Cr.Enums;
    using Bars.Gkh.Utils;

    using GkhCr.DomainService;

    public class DefectController : BaseController
    {
        protected IDefectService DefectService;

        public DefectController(IDefectService defectService)
        {
            DefectService = defectService;
        }

        public ActionResult WorksForDefectList(BaseParams baseParams)
        {
            var result = (ListDataResult)DefectService.WorksForDefectList(baseParams);
            return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult UseTypeDefectList(BaseParams baseParams)
        {
            var useTypeDefectList = Container.GetGkhConfig<GkhCrConfig>().General.DefectListUsage == DefectListUsage.Use;
            return new JsonNetResult(new
            {
                UseTypeDefectList = useTypeDefectList
            });
        }

        public ActionResult CalcInfo(BaseParams baseParams)
        {
            return new JsonNetResult(DefectService.CalcInfo(baseParams));
        }

        public ActionResult GetDefectListViewValue(BaseParams baseParams)
        {
            return new JsonNetResult(DefectService.GetDefectListViewValue(baseParams));
        }
    }
}