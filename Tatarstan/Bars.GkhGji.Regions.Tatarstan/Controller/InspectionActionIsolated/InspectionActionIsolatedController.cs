namespace Bars.GkhGji.Regions.Tatarstan.Controller.InspectionActionIsolated
{
    using System;
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.InspectionActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.InspectionActionIsolated;

    public class InspectionActionIsolatedController : B4.Alt.DataController<InspectionActionIsolated>
    {
        public ActionResult ListTaskActionIsolated(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IInspectionActionIsolatedService>();

            using (this.Container.Using(service))
            {
                return service.ListTaskActionIsolated(baseParams).ToJsonResult();
            }
        }

        /// <summary>
        /// Экспорт данных реестра "Проверки по КНМ без взаимодействия" в Excel файл
        /// </summary>
        public ActionResult Export(BaseParams baseParams)
        {
            var inspectionActionIsolatedExport = this.Container.Resolve<IDataExportService>("InspectionActionIsolatedExport");

            using (this.Container.Using(inspectionActionIsolatedExport))
            {
                try
                {
                    return inspectionActionIsolatedExport.ExportData(baseParams);
                }
                catch (Exception ex)
                {
                    return JsonNetResult.Failure(ex.Message);
                }
            }
        }
    }
}