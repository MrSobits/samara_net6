namespace Bars.GkhGji.Regions.Tatarstan.Controller.ActionIsolated
{
    using System;
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel.ActionIsolated;

    public class TaskActionIsolatedController : B4.Alt.DataController<TaskActionIsolated>
    {
        #region Dependency Injection
        private readonly IDomainService<TaskActionIsolated> taskActionIsolatedDomain;
        private readonly ITaskActionIsolatedService taskActionIsolatedService;

        public TaskActionIsolatedController(IDomainService<TaskActionIsolated> taskActionIsolatedDomain,
            ITaskActionIsolatedService taskActionIsolatedService)
        {
            this.taskActionIsolatedDomain = taskActionIsolatedDomain;
            this.taskActionIsolatedService = taskActionIsolatedService;
        }
        #endregion
        
        public ActionResult ListForCitizenAppeal(BaseParams baseParams)
        {
            var result = this.taskActionIsolatedService.ListForCitizenAppeal(baseParams);
            
            return result != null ? new JsonListResult((IEnumerable) result.Data) : JsonListResult.EmptyList;
        }

        /// <summary>
        /// Экспорт данных реестра "КНМ без взаимодействия с контролируемыми лицами" в Excel файл
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult Export(BaseParams baseParams)
        {
            var taskActionIsolatedExport = this.Container.Resolve<IDataExportService>("TaskActionIsolatedExport");

            using (this.Container.Using(taskActionIsolatedExport))
            {
                try
                {
                    return taskActionIsolatedExport.ExportData(baseParams);
                }
                catch (Exception ex)
                {
                    return JsonNetResult.Failure(ex.Message);
                }
            }
        }

        /// <summary>
        /// Получить список для реестра документов ГЖИ
        /// </summary>
        public ActionResult ListForDocumentRegistry(BaseParams baseParams)
        {
            return new JsonNetResult((this.ViewModel as TaskActionIsolatedViewModel).ListForDocumentRegistry(this.taskActionIsolatedDomain, baseParams));
        }
    }
}