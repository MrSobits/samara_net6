namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using DomainService;
    using Bars.GkhGji.Entities;
    using Entities;
    using Bars.Gkh.Domain;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.DomainService;

    public class AdmonitionOperationsController : BaseController
    {
        public IBlobPropertyService<AppealCitsAdmonition, AppealCitsAdmonitionLongText> LongTextService { get; set; }
        public ActionResult ListAdmonitionForSelect(BaseParams baseParams)
        {
            var service = Container.Resolve<IAdmonitionOperationsService>();
            try
            {
                var result = service.ListDocsForSelect(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult Export(BaseParams baseParams)
        {
            IDataExportService export = null;
            try
            {
                export = this.Container.Resolve<IDataExportService>("AdmonitionDataExport");
                return export.ExportData(baseParams);
            }
            finally
            {
                if (export != null)
                {
                    this.Container.Release(export);
                }
            }
        }

        /// <summary>
        /// Добавить обращение
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        public ActionResult SaveAppeal(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IAdmonitionOperationsService>();
            try
            {
                var result = service.SaveAppeal(baseParams);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Добавить обращение
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        public ActionResult SaveViolations(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IAdmonitionOperationsService>();
            try
            {
                var result = service.SaveViolations(baseParams);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public ActionResult RemoveRelated(long appealNumber, long admonId)
        {
            var service = this.Resolve<IAdmonitionOperationsService>();
            try
            {
                var result = service.RemoveRelated(admonId, appealNumber);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public virtual ActionResult GetDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Get(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public virtual ActionResult SaveDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Save(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }


    }
}