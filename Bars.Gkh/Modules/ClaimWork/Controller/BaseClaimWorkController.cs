namespace Bars.Gkh.Modules.ClaimWork.Controllers
{
    using System.Linq;
    using B4.Modules.FileStorage;
    using B4.Utils;
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.B4;
    using Bars.Gkh.Modules.ClaimWork.Export;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Базовый контроллер для всех оснований претензеонно исковой работы
    /// </summary>
    public class BaseClaimWorkController<T> : FileStorageDataController<T>
        where T : BaseClaimWork
    {
        /// <summary>
        /// Метод экспорта рестра
        /// </summary>
        public ActionResult Export(BaseParams baseParams)
        {
            var exportService = this.Container.Resolve<IBaseClaimWorkExport<T>>();

            try
            {
                return exportService.ExportData(baseParams);
            }
            finally
            {
                this.Container.Release(exportService);
            }
        }


        /// <summary>
        /// Массовое создание документов
        /// </summary>
        public ActionResult MassCreateDocs(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IBaseClaimWorkService<T>>();

            try
            {
                return new JsonNetResult(service.MassCreateDocs(baseParams));
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Метод обновления статусов
        /// </summary>
        public ActionResult UpdateStates(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IBaseClaimWorkService<T>>();

            try
            {
                var inTask = baseParams.Params.GetAs("inTask", true);

                var result = service.UpdateStates(baseParams, inTask);
                return new JsonNetResult(new { success = result.Success, message = result.Message, data = result.Data });
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Метод получения списка необходимых документов
        /// </summary>
        public ActionResult GetListRules(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IBaseClaimWorkService<T>>();
            var rules = this.Container.ResolveAll<IClaimWorkDocRule>();
            
            try
            {
                var needDocs = service.GetNeedDocs(baseParams);

                var result = rules
                    .Where(x => needDocs.Contains(x.ResultTypeDocument))
                    .Select(x => new
                    {
                        x.Id,
                        Name = x.ResultTypeDocument.GetDisplayName(),
                        x.ActionUrl
                    })
                    .ToList();

                return new JsonNetResult(result);
                
            }
            finally
            {
                this.Container.Release(service);
                this.Container.Release(rules);
            }
        }

        /// <summary>
        /// Метод получения списка необходимых документов
        /// </summary>
        public ActionResult GetDocsTypeToCreate(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IBaseClaimWorkService<T>>();
            try
            {
                var result = service.GetDocsTypeToCreate();

                return new JsonNetResult(result.Data);
            }
            finally
            {
                this.Container.Release(service);
            }
        }
    }
}