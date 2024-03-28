namespace Bars.GisIntegration.UI.Controllers
{
    using System.Collections;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GisIntegration.Base.Controllers;
    using Bars.GisIntegration.UI.Service;

    /// <summary>
    /// Контроллер для получения объектов при выполнении импорта/экспорта данных через сервис Bills
    /// </summary>
    public class BillsController : BaseDataSupplierController
    {
        /// <summary>
        /// Сервис для получения объектов при выполнении импорта/экспорта данных через сервис Bills
        /// </summary>
        public IBillsService BillsService { get; set; }
        
        /// <summary>
        /// Метод возвращает запросы на проведения квитирования
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Список запросов</returns>
        public ActionResult GetAcknowledgments(BaseParams baseParams)
        {
            var result = (ListDataResult)this.BillsService.GetAcknowledgments(baseParams);
            return result.Success ? new JsonListResult((IEnumerable)result.Data, result.TotalCount) : this.JsFailure(result.Message);
        }
    }
}
