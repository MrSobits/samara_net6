namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Overhaul.Hmao.Task;
    using Castle.Windsor;

    public class EconFesabilityCalcController : BaseController
    {

        /// <summary>
        /// Менеджер задач
        /// </summary>
        private readonly ITaskManager _taskManager;

        private readonly IWindsorContainer _container;

        public EconFesabilityCalcController(IWindsorContainer container, ITaskManager taskManager)
        {
            _container = container;         
            _taskManager = taskManager;
        }

        public virtual ActionResult ExecuteCalculation(BaseParams baseParams)
        {
            try
            {
                var taskInfo = _taskManager.CreateTasks(new EconFesabilityCalcTaskProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
                if (taskInfo == null)
                    return JsFailure("Сбой создания задачи");
                else
                    return JsSuccess($"Задача на расчет целесообразности поставлена в очередь с id {taskInfo.TaskId}");
            }
            catch (Exception e)
            {
                return JsFailure("Ошибка при расчете: " + e.Message);
            }

        }
    }
}