namespace Bars.Gkh.Overhaul.Tat.Controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Overhaul.Tat.DomainService;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class DpkrCorrectionStage2Controller : B4.Alt.DataController<DpkrCorrectionStage2>
    {
        private IDpkrCorrectionService _service;

        public DpkrCorrectionStage2Controller(IDpkrCorrectionService service)
        {
            _service = service;
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("DpkrCorrectionStage2Export");
            return export != null ? export.ExportData(baseParams) : null;
        }

        public JsonListResult GetActualizeYears(BaseParams baseParams)
        {
            var result = _service.GetActualizeYears(baseParams);
            var list = (List<int>)result.Data;

            return new JsonListResult(list.Select(x => new
            {
                Year = x
            }).ToList());
        }

        public ActionResult GetInfo(BaseParams baseParams)
        {
            var result = _service.GetInfo(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ChangeIndexNumber(BaseParams baseParams)
        {
            var result = _service.ChangeIndexNumber(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetHistory(BaseParams baseParams)
        {
            var result = (ListDataResult)_service.GetHistory(baseParams);
            return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);           
        }

        public ActionResult GetHistoryDetail(BaseParams baseParams)
        {
            var result = (ListDataResult)_service.GetHistoryDetail(baseParams);
            return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);            
        }

        public ActionResult ListForMassChangeYear(BaseParams baseParams)
        {
            var result = (ListDataResult)_service.ListForMassChangeYear(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        public ActionResult MassChangeYear(BaseParams baseParams)
        {
            var result = _service.MassChangeYear(baseParams);
            return result.Success ? JsSuccess(result.Data) : JsFailure(result.Message);
        }
    }
}