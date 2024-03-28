namespace Bars.GkhGji.Regions.Tatarstan.Controller.InspectionPreventiveAction
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.InspectionPreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.InspectionPreventiveAction;

    /// <summary>
    /// Контроллер для <see cref="InspectionPreventiveAction"/>
    /// </summary>
    public class InspectionPreventiveActionController : B4.Alt.DataController<InspectionPreventiveAction>
    {
        /// <summary>
        /// Список профилактических мероприятий
        /// </summary>
        public ActionResult ListPreventiveAction(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IInspectionPreventiveActionService>();

            using (this.Container.Using(service))
            {
                return service.ListPreventiveAction(baseParams).ToJsonResult();
            }
        }

        /// <summary>
        /// Экспорт данных в Excel
        /// </summary>
        public ActionResult Export(BaseParams baseParams)
        {
            var exportService = this.Container.Resolve<IDataExportService>("InspectionPreventiveActionExport");

            using (this.Container.Using(exportService))
            {
                return exportService.ExportData(baseParams);
            }
        }
    }
}