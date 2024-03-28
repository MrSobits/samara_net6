namespace Bars.GisIntegration.Base.Controllers
{
    using System.Collections;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Service;

    /// <summary>
    /// Контроллер настроек контекстов
    /// </summary>
    public class ContextSettingsController : B4.Alt.DataController<ContextSettings>
    {
        /// <summary>
        /// Сервис для работы с настройками интеграции ГИС
        /// </summary>
        public IGisSettingsService GisSettingsService { get; set; }

        /// <summary>
        /// Получить список сохраненных настроек контекстов
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult GetStorableSettings(BaseParams baseParams)
        {
            var result = (ListDataResult)this.GisSettingsService.GetStorableContextSettings(baseParams);
            return new JsonListResult((IEnumerable)result.Data, result.TotalCount);
        }

        /// <summary>
        /// Метод возвращает хранилища данных ГИС, для которых не настроены контексты 
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult GetMissedSettings(BaseParams baseParams)
        {
            var result = (ListDataResult)this.GisSettingsService.GetMissedContextSettings(baseParams);
            return new JsonListResult((IEnumerable)result.Data, result.TotalCount);
        }
    }
}
