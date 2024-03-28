namespace Bars.GisIntegration.UI.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GisIntegration.Base.Controllers;
    using Bars.GisIntegration.UI.Service;

    /// <summary>
    /// Контроллер для работы с оплатами
    /// </summary>
    public class PaymentServiceController : BaseDataSupplierController
    {
        /// <summary>
        /// Сервис работы с оплатами
        /// </summary>
        public IPaymentService PaymentService { get; set; }

        /// <summary>
        /// Вернуть список распоряжений, помеченных к удалению из ГИСа
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public ActionResult GetNotificationsToDelete(BaseParams baseParams)
        {
            return new JsonNetResult(this.PaymentService.GetNotificationsToDelete(baseParams));
        }

        /// <summary>
        /// Получить список распоряжений
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public ActionResult GetNotificationsToAdd(BaseParams baseParams)
        {
            var result = this.PaymentService.GetNotificationsToAdd(baseParams);
            return result.Success ? this.JsSuccess(result) : this.JsFailure(result.Message);
        }
    }
}