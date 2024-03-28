namespace Bars.GkhGji.Regions.Tatarstan.Controller.PreventiveAction
{
    using System;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.PreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    /// <summary>
    /// Контроллер для <see cref="VisitSheet"/>
    /// </summary>
    public class VisitSheetController : B4.Alt.DataController<VisitSheet>
    {
        /// <summary>
        /// Получить МО для документа "Лист визита"
        /// </summary>
        public ActionResult GetVisitSheetMunicipality(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IVisitSheetService>();
            using (this.Container.Using(service))
            {
                return service.GetVisitSheetMunicipality(baseParams).ToJsonResult();
            }
        }

        /// <summary>
        /// Получить список домов из Нарушений по документу "Лист визита"
        /// </summary>
        public ActionResult GetViolationRealityObjectsList(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IVisitSheetService>();
            using (this.Container.Using(service))
            {
                return service.GetViolationRealityObjectsList(baseParams).ToJsonResult();
            }
        }
        
        /// <summary>
        /// Получить список для реестра документов ГЖИ
        /// </summary>
        public ActionResult ListForRegistry(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IVisitSheetService>();
            using (this.Container.Using(service))
            {
                return service.ListForRegistry(baseParams).ToJsonResult();
            }
        }
        
        /// <summary>
        /// Экспорт данных реестра "Лист визита" в Excel файл
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult Export(BaseParams baseParams)
        {
            var visitSheetDataExport = this.Container.Resolve<IDataExportService>("VisitSheetDataExport");

            using (this.Container.Using(visitSheetDataExport))
            {
                try
                {
                    return visitSheetDataExport.ExportData(baseParams);
                }
                catch (Exception ex)
                {
                    return JsonNetResult.Failure(ex.Message);
                }
            }
        }
    }
}