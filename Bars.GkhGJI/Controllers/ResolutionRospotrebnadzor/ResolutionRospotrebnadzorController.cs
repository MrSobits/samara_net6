namespace Bars.GkhGji.Controllers.ResolutionRospotrebnadzor
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Контроллер Постановление Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzorController : ResolutionRospotrebnadzorController<ResolutionRospotrebnadzor>
    {
    }

    /// <summary>
    /// Generic контроллер Постановление Роспотребнадзора
    /// </summary>
    /// <typeparam name="T"><see cref="ResolutionRospotrebnadzor"/></typeparam>
    public class ResolutionRospotrebnadzorController<T> : Bars.B4.Alt.DataController<T>
        where T : ResolutionRospotrebnadzor
    {
        /// <summary>
        /// Получить постановление Роспотребнадзора по ID документа
        /// </summary>
        /// <param name="documentId">ID ГЖИ документа</param>
        public ActionResult GetInfo(long? documentId)
        {
            var service = this.Container.Resolve<IResolutionRospotrebnadzorService>();
            var result = service.GetInfo(documentId);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Список постановлений Роспотребнадзора
        /// </summary>
        public ActionResult ListView(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IResolutionRospotrebnadzorService>();
            var result = (ListDataResult)service.ListView(baseParams);
            return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Экспорт постановлений Роспотребнадзора
        /// </summary>
        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("ResolutionRospotrebnadzorDataExport");

            try
            {
                return export.ExportData(baseParams);
            }
            finally
            {
                this.Container.Release(export);
            }
        }

        /// <summary>
        /// Добавить список статей закона к постановлению Роспотребнадзора
        /// </summary>
        public ActionResult AddArticleLawList(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IResolutionRospotrebnadzorService>();
            var result = service.AddArticleLawList(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }
    }
}