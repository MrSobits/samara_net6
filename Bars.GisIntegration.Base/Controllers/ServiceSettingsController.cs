namespace Bars.GisIntegration.Base.Controllers
{
    using System.Collections;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Service;

    /// <summary>
    /// Контроллер для работы с настройками интеграции ГИС
    /// </summary>
    public class ServiceSettingsController : B4.Alt.DataController<ServiceSettings>
    {
        /// <summary>
        /// Сервис для работы с настройками интеграции ГИС
        /// </summary>
        public IGisSettingsService GisSettingsService { get; set; }

        /// <summary>
        /// Вернуть данные для Единых настроек
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult GetRegisteredSettings(BaseParams baseParams)
        {
            var result = (ListDataResult)this.GisSettingsService.GetRegisteredSettings(false);
            return new JsonListResult((IEnumerable)result.Data, result.TotalCount);
        }

        /// <summary>
        /// Метод возвращает типы сервисов, которые не настроены
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult GetMissedSettings(BaseParams baseParams)
        {
            var result = (ListDataResult)this.GisSettingsService.GetMissedSettings(baseParams);
            return new JsonListResult((IEnumerable)result.Data, result.TotalCount);
        }
    }
}