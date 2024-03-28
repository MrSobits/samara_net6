namespace Bars.B4.Modules.Analytics.Reports.Web.Controllers
{
    using System.Net.Mime;
    using Bars.B4.Modules.Analytics.Reports.Domain;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер генератора печати
    /// </summary>
    public class ReportGeneratorController : BaseController
    {
        /// <summary>
        /// Интерфейс сервиса генерации отчётов
        /// </summary>
        public IReportGeneratorService ReportGeneratorService { get; set; }

        /// <summary>
        /// Сгенерировать печать
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public ActionResult Generate(BaseParams baseParams)
        {
            var reportResult = this.ReportGeneratorService.Generate(baseParams);

            return this.File(reportResult.FileName, MediaTypeNames.Application.Octet, reportResult.FileName);
        }

        /// <summary>
        /// Сохранить на сервер
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public ActionResult SaveOnServer(BaseParams baseParams)
        {
            var result = this.ReportGeneratorService.CreateTaskOrSaveOnServer(baseParams);

            if (result.Success)
            {
                return new JsonNetResult(result.Data);
            }

            return this.JsFailure(result.Message ?? "Ошибка печати отчета");
        }
    }
}