namespace Bars.GkhGji.Regions.Tatarstan.Controller.ActionIsolated
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    /// <summary>
    /// Контроллер для <see cref="ActActionIsolated"/>
    /// </summary>
    public class ActActionIsolatedController : B4.Alt.DataController<ActActionIsolated>
    {
        /// <summary>
        /// Сохранить дома по проверке
        /// </summary>
        public ActionResult SaveRealityObjects(BaseParams baseParams)
        {
            var actActionIsolatedService = this.Container.Resolve<IActActionIsolatedService>();

            using (this.Container.Using(actActionIsolatedService))
            {
                var result = actActionIsolatedService.SaveRealityObjects(baseParams);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
        }

        /// <summary>
        /// Получить список домов из задачи
        /// </summary>
        public ActionResult GetRealityObjectsList(BaseParams baseParams)
        {
            var actActionIsolatedService = this.Container.Resolve<IActActionIsolatedService>();

            using (this.Container.Using(actActionIsolatedService))
            {
                var result = actActionIsolatedService.GetRealityObjectsList(baseParams);
                return result.Success ? new JsonListResult((IEnumerable) result.Data) : JsonNetResult.Failure(result.Message);
            }
        }

        /// <summary>
        /// Получить список домов для определения
        /// </summary>
        public ActionResult GetRealityObjectsForDefinition(BaseParams baseParams)
        {
            var actActionIsolatedService = this.Container.Resolve<IActActionIsolatedService>();

            using (this.Container.Using(actActionIsolatedService))
            {
                var result = actActionIsolatedService.GetRealityObjectsForDefinition(baseParams);
                return result.Success ? new JsonListResult((IEnumerable) result.Data) : JsonNetResult.Failure(result.Message);
            }
        }

        /// <summary>
        /// Получить список актов по КНМ без взаимодействия для реестра
        /// </summary>
        public ActionResult ListForRegistry(BaseParams baseParams)
        {
            var actActionIsolatedService = this.Container.Resolve<IActActionIsolatedService>();
            using (this.Container.Using(actActionIsolatedService))
            {
                return actActionIsolatedService.ListForRegistry(baseParams).ToJsonResult();
            }
        }

        /// <summary>
        /// Экспорт реестра в Excel файл
        /// </summary>
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("ActActionIsolatedExport");

            using (this.Container.Using(export))
            {
                return export.ExportData(baseParams);
            }
        }
    }
}