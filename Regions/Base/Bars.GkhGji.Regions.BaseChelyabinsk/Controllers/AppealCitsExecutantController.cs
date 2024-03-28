namespace Bars.GkhGji.Regions.BaseChelyabinsk.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;

    public class AppealCitsExecutantController : FileStorageDataController<AppealCitsExecutant>
    {
        public ActionResult AddExecutants(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IAppealCitsExecutantService>().AddExecutants(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult RedirectExecutant(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IAppealCitsExecutantService>().RedirectExecutant(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListAppealOrderExecutant(BaseParams baseParams)
        {
            var appealService = Container.Resolve<IAppealCitsExecutantService>();
            try
            {
                return appealService.ListAppealOrderExecutant(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }

        /// <summary>
        /// Экспорт резолюций
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        public ActionResult Export(BaseParams baseParams)
        {
            var exportService = this.Container.Resolve<IAppealCitsExecutantDataExport>();
            using (this.Container.Using(exportService))
            {
                return exportService.ExportData(baseParams);
            }
        }

        public ActionResult GetSOPRId(BaseParams baseParams)
        {
            var erpService = Container.Resolve<IAppealCitsExecutantService>();
            try
            {
                var data = erpService.GetSOPRId(baseParams);
                return JsSuccess(data);
            }
            finally
            {

            }
        }
    }
}