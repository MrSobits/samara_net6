namespace Bars.GkhCr.Regions.Tatarstan.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhCr.Regions.Tatarstan.DomainService;
    using Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr;

    public class ObjectOutdoorCrController : B4.Alt.DataController<ObjectOutdoorCr>
    {
        /// <summary>
        /// Восстановить.
        /// </summary>
        public ActionResult Recover(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IObjectOutdoorCrService>();
            using (this.Container.Using(service))
            {
                var result = service.Recover(baseParams);
                return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
            }
        }

        /// <summary>
        /// Выгрузка в Excel.
        /// </summary>
        public ActionResult ExportToExcel(BaseParams baseParams)
            => this.Container.Resolve<IDataExportService>("ObjectOutdoorCrDataExport").ExportData(baseParams);
    }
}
