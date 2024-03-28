namespace Bars.GisIntegration.UI.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GisIntegration.Base.Controllers;
    using Bars.GisIntegration.UI.Service;

    /// <summary>
    /// Контроллер для получения объектов при выполнении импорта/экспорта данных через сервис CapitalRepair
    /// </summary>
    public class CapitalRepairController : BaseDataSupplierController
    {
        /// <summary>
        /// Сервис для получения объектов при выполнении импорта/экспорта данных через сервис CapitalRepair
        /// </summary>
        public ICapitalRepairService CapitalRepairService { get; set; }

        /// <summary>
        /// Получить список МО
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult GetMunicipalities(BaseParams baseParams)
        {
            var result = this.CapitalRepairService.GetMunicipalities(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получить список планов по кап. ремонту
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult GetCapitalRepairPlan(BaseParams baseParams)
        {
            var result = this.CapitalRepairService.GetCapitalRepairPlan(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }
    }
}
