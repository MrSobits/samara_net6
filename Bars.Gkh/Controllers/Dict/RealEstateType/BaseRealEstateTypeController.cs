namespace Bars.Gkh.Controllers.Dict.RealEstateType
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.DomainService.Dict.RealEstateType;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Базовый контроллер.
    /// !Не регается в контейнере, нужен только для общего метода. наследуется в региональных модулях (Hmao, Nso, Tat)
    /// </summary>
    public class BaseRealEstateTypeController : B4.Alt.DataController<RealEstateType>
    {
        /// <summary>
        /// Оббновления типов домов
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат</returns>
        public ActionResult UpdateTypes(BaseParams baseParams)
        {
            var result = this.Resolve<IRealEstateTypeService>().UpdateRobjectTypes();
            if (!result.Success)
            {
                return this.JsFailure(result.Message);
            }

            return this.JsSuccess();
        }

        /// <summary>
        /// Предпросмотр результатов обновления типов домов
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат</returns>
        public ActionResult UpdateTypesPreview(BaseParams baseParams)
        {
            var service = this.Resolve<IRealEstateTypeService>();

            using (this.Container.Using(service))
            {
                var result = (ListDataResult) service.UpdateRobjectTypesPreview(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
        }

        /// <summary>
        /// Экспорт
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат</returns>
        public ActionResult UpdateTypesPreviewExport(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("UpdateRoTypesPreviewExport");
            try
            {
                return export?.ExportData(baseParams);
            }
            finally
            {
                this.Container.Release(export);
            }
        }
    }
}