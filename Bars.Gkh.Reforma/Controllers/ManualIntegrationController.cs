namespace Bars.Gkh.Reforma.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Reforma.Domain;

    /// <summary>
    /// Контроллер для ручной интеграцией с реформой ЖКХ
    /// </summary>
    public class ManualIntegrationController : BaseController
    {
        /// <summary>
        /// Сервис для работы с выборочной интеграцией Реформы.ЖКХ
        /// </summary>
        public IManualIntegrationService ManualIntegrationService { get; set; }

        /// <summary>
        /// Добавить в очередь задачу интеграции УО
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult ScheduleManorgIntegrationTask(BaseParams baseParams)
        {
            var result = this.ManualIntegrationService.ScheduleManorgIntegrationTask(baseParams);
            return result.Success ? this.JsSuccess(result) : this.JsFailure(result.Message);
        }

        /// <summary>
        /// Добавить в очередь задачу интеграции домов
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult ScheduleRobjectIntegrationTask(BaseParams baseParams)
        {
            var result = this.ManualIntegrationService.ScheduleRobjectIntegrationTask(baseParams);
            return result.Success ? this.JsSuccess(result) : this.JsFailure(result.Message);
        }

        /// <summary>
        /// Вернуть список управляемых домов для выполнения выборочной интеграции
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult ListManagedRealityObjects(BaseParams baseParams)
        {
            var result = (ListDataResult)this.ManualIntegrationService.ListManagedRealityObjects(baseParams);
            return result.Success ? new JsonListResult((IEnumerable)result.Data, result.TotalCount) : this.JsFailure(result.Message);
        }
    }
}