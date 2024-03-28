namespace Bars.Gkh.Controllers
{
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction.ExecutionActionScheduler;
    using Bars.Gkh.Utils;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер выполнения действий
    /// </summary>
    public class ExecutionActionController : BaseController
    {
        public IExecutionActionInfoService ExecutionActionInfoService { get; set; }
        public IExecutionActionService ExecutionActionService { get; set; }

        /// <summary>
        /// Получить список действий
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Список действий</returns>
        public virtual ActionResult List(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            return this.ExecutionActionInfoService.GetInfoAll()
                .ToListDataResult(loadParams, this.Container)
                .ToJsonResult();
        }

        public virtual ActionResult GetQueueInfo(BaseParams baseParams)
        {
            return this.ExecutionActionService.GetSchedulerQueue().ToJsonResult();
        }
    }
}