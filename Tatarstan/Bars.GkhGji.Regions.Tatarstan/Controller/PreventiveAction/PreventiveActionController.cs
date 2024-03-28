namespace Bars.GkhGji.Regions.Tatarstan.Controller.PreventiveAction
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel.PreventiveAction;

    /// <summary>
    /// Контроллер для <see cref="PreventiveAction"/>
    /// </summary>
    public class PreventiveActionController : B4.Alt.DataController<PreventiveAction>
    {
        /// <summary>
        /// Получить список для реестра документов ГЖИ
        /// </summary>
        public ActionResult ListForDocumentRegistry(BaseParams baseParams)
        {
            var service = this.Container.ResolveDomain<PreventiveAction>();

            using (this.Container.Using(service))
            {
                return new JsonNetResult((this.ViewModel as PreventiveActionViewModel).ListForDocumentRegistry(service, baseParams));
            }
        }

        /// <summary>
        /// Экспорт реестра в Excel файл
        /// </summary>
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("PreventiveActionExport");

            using (this.Container.Using(export))
            {
                return export.ExportData(baseParams);
            }
        }
    }
}