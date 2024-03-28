namespace Bars.GisIntegration.UI.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GisIntegration.Base.Controllers;
    using Bars.GisIntegration.UI.Service;

    /// <summary>
    /// Контроллер для получения объектов при выполнении импорта/экспорта данных через сервис OrgRegistry
    /// </summary>
    public class OrgRegistryController : BaseDataSupplierController
    {
        /// <summary>
        /// Сервис для получения объектов при выполнении импорта/экспорта данных через сервис OrgRegistry
        /// </summary>
        public IOrgRegistryService OrgRegistryService { get; set; }

        /// <summary>
        /// Получить список контрагентов
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult GetContragentList(BaseParams baseParams)
        {
            var result = this.OrgRegistryService.GetContragentList(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }
    }
}
