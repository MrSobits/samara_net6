using Bars.B4;
using Bars.B4.IoC;
using Bars.B4.Modules.DataExport.Domain;
using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;
using Microsoft.AspNetCore.Mvc;

namespace Bars.GkhGji.Regions.Tatarstan.Controller.PreventiveAction
{
    /// <summary>
    /// Контроллер для <see cref="PreventiveActionTask"/>
    /// </summary>
    public class PreventiveActionTaskController : B4.Alt.DataController<PreventiveActionTask>
    {
        /// <summary>
        /// Экспорт реестра в Excel файл
        /// </summary>
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("PreventiveActionTaskExport");
            using (Container.Using(export))
            {
                return export.ExportData(baseParams);
            }
        }
    }
}