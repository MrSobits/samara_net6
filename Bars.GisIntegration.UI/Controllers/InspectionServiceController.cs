namespace Bars.GisIntegration.UI.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GisIntegration.Base.Controllers;
    using Bars.GisIntegration.UI.Service;

    /// <summary>
    /// Контроллер для получения объектов при выполнении импорта/экспорта данных через сервис Inspection
    /// </summary>
    public class InspectionServiceController : BaseDataSupplierController
    {
        /// <summary>
        /// Сервис для получения объектов при выполнении импорта/экспорта данных через сервис Inspection
        /// </summary>
        public IInspectionService InspectionService { get; set; }

        /// <summary>
        /// Получить список домов
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult GetPlanList(BaseParams baseParams)
        {
            var result = this.InspectionService.GetPlanList(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получить список инспекционных проверок
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult GetInspectionList(BaseParams baseParams)
        {
            var result = this.InspectionService.GetInspectionList(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }
    }
}
