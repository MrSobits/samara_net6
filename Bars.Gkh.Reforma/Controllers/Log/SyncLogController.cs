namespace Bars.Gkh.Reforma.Controllers.Log
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Reforma.Domain;

    /// <summary>
    /// Лог интеграции
    /// </summary>
    public class SyncLogController : BaseController
    {
        /// <summary>
        /// Сервис лога
        /// </summary>
        private ISyncLogService Service { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="service">Сервис лога</param>
        public SyncLogController(ISyncLogService service)
        {
            this.Service = service;
        }

        /// <summary>
        /// Список сессий
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult ListSessions(BaseParams baseParams)
        {
            var result = this.Service.ListSessions(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Список действий в рамках сессии
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult ListActions(BaseParams baseParams)
        {
            var result = this.Service.ListActions(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Детали действия
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult GetActionDetails(BaseParams baseParams)
        {
            var result = this.Service.GetActionDetails(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Повтор действия
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult ReplayAction(BaseParams baseParams)
        {
            var result = this.Service.ReplayAction(baseParams);
            return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
        }
    }
}