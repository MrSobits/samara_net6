namespace Bars.GisIntegration.UI.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GisIntegration.Base.Controllers;
    using Bars.GisIntegration.UI.Service;

    /// <summary>
    /// Контроллер для получения объектов при выполнении импорта/экспорта данных через сервис HouseManagement
    /// </summary>
    public class HouseManagementController : BaseDataSupplierController
    {
        /// <summary>
        /// Сервис для получения объектов при выполнении импорта/экспорта данных через сервис HouseManagement
        /// </summary>
        public IHouseManagementService HouseManagementService { get; set; }

        /// <summary>
        /// Получить список домов
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult GetHouseList(BaseParams baseParams)
        {
            var result = this.HouseManagementService.GetHouseList(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получить список договоров
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult GetContractList(BaseParams baseParams)
        {
            var result = this.HouseManagementService.GetContractList(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получить список уставов
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult GetCharterList(BaseParams baseParams)
        {
            var result = this.HouseManagementService.GetCharterList(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получить список ДОИ
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult GetPublicPropertyContractList(BaseParams baseParams)
        {
            var result = this.HouseManagementService.GetPublicPropertyContractList(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получить список новостей
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult GetNotificationList(BaseParams baseParams)
        {
            var result = this.HouseManagementService.GetNotificationList(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получить список договоров ресурсоснабжения
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult GetSupplyResourceContractList(BaseParams baseParams)
        {
            var result = this.HouseManagementService.GetSupplyResourceContractList(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }
    }
}
