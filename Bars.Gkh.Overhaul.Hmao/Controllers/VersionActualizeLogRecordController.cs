namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Overhaul.Hmao.Entities.Version;

    /// <summary>
    /// Контроллер для <see cref="VersionActualizeLogRecord"/>
    /// </summary>
    public class VersionActualizeLogRecordController : B4.Alt.DataController<VersionActualizeLogRecord>
    {
        /// <summary>
        /// Экспорт в Excel
        /// </summary>
        public ActionResult Export(BaseParams baseParams)
        {
            var exportService = this.Container.Resolve<IDataExportService>("VersionActualizeLogRecordExport");

            using (this.Container.Using(exportService))
            {
                return exportService?.ExportData(baseParams);
            }
        }
    }
}