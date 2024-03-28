namespace Bars.GisIntegration.UI.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GisIntegration.Base.Controllers;
    using Bars.GisIntegration.UI.Service;

    /// <summary>
    /// Контроллер для получения объектов при выполнении экспорта/импорта данных через сервис Services
    /// </summary>
    public class ServicesController : BaseDataSupplierController
    {
        /// <summary>
        /// Сервис для получения объектов при выполнении экспорта/импорта данных через сервис Services
        /// </summary>
        public IServicesService ServicesService { get; set; }

        /// <summary>
        /// Метод получения списка объектов текущего ремонта
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult GetRepairObjectList(BaseParams baseParams)
        {
            var result = this.ServicesService.GetRepairObjectList(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Метод возвращает список активных программ текущего ремонта
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult GetRepairProgramList(BaseParams baseParams)
        {
            var result = this.ServicesService.GetRepairProgramList(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Метод возвращает список актов выполненных работ
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult GetCompletedWorkList(BaseParams baseParams)
        {
            var result = this.ServicesService.GetCompletedWorkList(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }
    }
}
