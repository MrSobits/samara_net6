namespace Bars.GkhCr.Controllers
{
    using System;
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Domain;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    public class TypeWorkCrController : B4.Alt.DataController<TypeWorkCr>
    {
        public IOverhaulViewModels OverhaulViewModels { get; set; }

        public IChangeVersionSt1Service ChangeVersionSt1Service { get; set; }

        /// <summary>
        /// метод для получения списка работ по жилому дому и периоду, используется
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult ListRealityObjectWorksByPeriod(BaseParams baseParams)
        {
            var listResult = (ListDataResult) this.Resolve<ITypeWorkCrService>().ListRealityObjectWorksByPeriod(baseParams);
            return new JsonNetResult(new {success = listResult.Success, data = listResult.Data, totalCount = listResult.TotalCount});
        }

        public ActionResult CalcPercentOfCompletion(BaseParams baseParams)
        {
            var result = this.Resolve<ITypeWorkCrService>().CalcPercentOfCompletion(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
        }

        public ActionResult ListByProgramCr(BaseParams baseParams)
        {
            var totalCount = 0;
            var list = this.Resolve<ITypeWorkCrService>().ListByProgramCr(baseParams, true, ref totalCount);
            return new JsonListResult(list, totalCount);
        }

        public ActionResult ListWorksCr(BaseParams baseParams)
        {
            var service = this.Resolve<IWorksCrService>();
            try
            {
                var result = (ListDataResult) service.List(baseParams);
                return new JsonListResult((IEnumerable) result.Data, result.TotalCount);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public ActionResult CreateTypeWork(BaseParams baseParams)
        {
            var service = this.Resolve<ITypeWorkCrService>();

            try
            {
                return service.CreateTypeWork(baseParams).ToJsonResult();
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public ActionResult MoveTypeWork(BaseParams baseParams, Int64 programId, Int64 typeworkToMoveId)
        {
            var service = this.Resolve<ITypeWorkCrService>();

            try
            {
                return service.MoveTypeWork(baseParams, programId, typeworkToMoveId).ToJsonResult();
                return JsSuccess();
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("WorksCrExport");
            return export != null ? export.ExportData(baseParams) : null;
        }

        public ActionResult TypeWorkStage1List(BaseParams baseParams)
        {
            if (this.OverhaulViewModels != null)
            {
                var result = (ListDataResult) this.OverhaulViewModels.TypeWorkStage1List(baseParams);
                return new JsonListResult((IEnumerable) result.Data, result.TotalCount);
            }

            return new JsonListResult(null, 0);
        }

        public ActionResult ChangeYear(BaseParams baseParams)
        {
            if (this.ChangeVersionSt1Service != null)
            {
                var result = this.ChangeVersionSt1Service.ChangeYear(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }

            return JsonNetResult.Success;
        }

        /// <summary>
        /// Разделить конструктивный элемент от вида работ
        /// </summary>
        /// <param name="baseParams"> baseParams </param>
        /// <returns> ActionResult </returns>
        public ActionResult SplitStructElementInTypeWork(BaseParams baseParams)
        {
            var dpkrTypeWorkService = this.Resolve<IDpkrTypeWorkService>();

            try
            {
                var result = dpkrTypeWorkService.SplitStructElementInTypeWork(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(dpkrTypeWorkService);
            }
        }
    }
}