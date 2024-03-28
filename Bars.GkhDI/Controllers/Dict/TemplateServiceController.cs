namespace Bars.GkhDi.Controllers
{
    using B4;

    using Bars.B4.Modules.DataExport.Domain;

    using DomainService;
    using Entities;

    using Microsoft.AspNetCore.Mvc;

    public class TemplateServiceController : B4.Alt.DataController<TemplateService>
    {
        /// <summary>
        /// Получаем имена настраиваемых полей (Только те которые нужно скрывать)
        /// </summary>
        /// <returns></returns>
        public ActionResult GetOptionsFields(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<ITemplateServService>().GetOptionsFields(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Метод строящий настраиваемые поля шаблонной услуги
        /// </summary>
        /// <returns></returns>
        public ActionResult ConstructOptionsFields(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<ITemplateServService>().ConstructOptionsFields(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получаем имена настраиваемых полей (Только те которые нужно скрывать)
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUnitMeasure(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<ITemplateServService>().GetUnitMeasure(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("TemplateServiceDataExport");

            if (export != null)
            {
                return export.ExportData(baseParams);
            }

            return null;
        }

        protected class OptionFieldData
        {
            /// <summary>
            /// Имя поля которое видит пользователь
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Имя поля
            /// </summary>
            public string FieldName { get; set; }
        }
    }
}
