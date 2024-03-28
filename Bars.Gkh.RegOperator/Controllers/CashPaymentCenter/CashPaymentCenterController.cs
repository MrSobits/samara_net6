namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Контроллер "Расчетно-кассовые центры"
    /// </summary>
    public class CashPaymentCenterController : B4.Alt.DataController<CashPaymentCenter>
    {
        /// <summary>
        /// Сервис расчётно-кассовых центров
        /// </summary>
        public ICashPaymentCenterService Service { get; set; }

        /// <summary>
        /// Список расчетно-кассовых центров без пагинации
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult ListWithoutPaging(BaseParams baseParams)
        {
            var result = (ListDataResult)Container.Resolve<IDeliveryAgentService>().ListWithoutPaging(baseParams);

            if (result.Success)
            {
                return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
            }

            return JsonNetResult.Message(result.Message);
        }

        /// <summary>
        /// Открепить дома к расчётно-кассовому центру
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult DeleteObjects(BaseParams baseParams)
        {
            var result = Service.DeleteObjects(baseParams);
            return result.ToJsonResult();
        }

        /// <summary>
        /// Добавить связь расчетно-кассового центра с МО
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult AddMunicipalities(BaseParams baseParams)
        {
            var result = Service.AddMunicipalities(baseParams);
            return result.ToJsonResult();
        }

        /// <summary>
        /// Прикрепить дома к расчётно-кассовому центру
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult AddObjects(BaseParams baseParams)
        {
            var result = Service.AddObjects(baseParams);
            return new JsonNetResult(result);
        }

        /// <summary>
        /// Установить расчётно-кассовый центр, из которого вызвана функция,
        /// всем Л/С без расчётно-кассового центра
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult SetCashPaymentCenters(BaseParams baseParams)
        {
            var result = Service.SetCashPaymentCenters(baseParams);
            return new JsonNetResult(result);
        }

        /// <summary>
        /// Список домов для прикрепления их к Расчетно-кассовому центру
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult ListObjForCashPaymentCenter(BaseParams baseParams)
        {
            return new JsonNetResult(Service.ListObjForCashPaymentCenter(baseParams));
        }

        /// <summary>
        /// Список домов в Расчетно-кассовом центре с управляющими компаниями
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult ListRealObjForCashPaymentCenterManOrg(BaseParams baseParams)
        {
            return new JsonNetResult(Service.ListRealObjForCashPaymentCenterManOrg(baseParams));
        }
    }
}