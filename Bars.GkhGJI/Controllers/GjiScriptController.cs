namespace Bars.GkhGji.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.DomainService;

    /// <summary>
    /// Контроллер для скриптов (контролелер для проведения какихто операций ГЖИ (которые нельзя выполнить скриптов в SQL))
    /// </summary>
    public class GjiScriptController : BaseController
    {
        public ActionResult MKDLicRequestExport(BaseParams baseParams)
        {
            IDataExportService export = null;
            try
            {
                export = this.Container.Resolve<IDataExportService>("MKDLicRequestExport");
                return export.ExportData(baseParams);
            }
            finally
            {
                if (export != null)
                {
                    this.Container.Release(export);
                }
            }
        }
        public ActionResult SetAddressAppeal(BaseParams baseParams)
        {
            var result = Container.Resolve<IGjiScriptService>().SetAddressAppeal(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GenerateFake(BaseParams baseParams)
        {
            var result = Container.Resolve<IGjiScriptService>().ReminderGenerateFake(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult SetZonalInspection(BaseParams baseParams)
        {
            var result = Container.Resolve<IGjiScriptService>().SetZonalInspection(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult CorrectDocNum(BaseParams baseParams)
        {
            var result = Container.Resolve<IGjiScriptService>().CorrectDocNumbers(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        public virtual ActionResult ListRealityObjectOnSpecAcc(BaseParams baseParams)
        {
            var service = Container.Resolve<IGjiScriptService>();
            try
            {
                var result = (ListDataResult)service.ListRealityObjectOnSpecAcc(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
        public ActionResult TrySetOpenEDS(BaseParams baseParams)
        {
            var service = Container.Resolve<IGjiScriptService>();
            try
            {
                var result = service.TrySetOpenEDS(baseParams);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}
