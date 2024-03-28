namespace Bars.Gkh.RegOperator.Tasks.Period.Callbacks
{
    using System.Reflection;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.RegOperator.Tasks.Period.Providers;

    using Castle.Windsor;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Обратный вызов при удачном завершении Закрытия периода (Этап 1)
    /// </summary>
    public class PeriodCloseSuccessCallback_Step1 : ITaskCallback
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private readonly IWindsorContainer container;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="container">Контейнер</param>
        public PeriodCloseSuccessCallback_Step1(IWindsorContainer container)
        {
            this.container = container;
        }

        #region Implementation of ITaskCallback
        /// <summary>
        /// Вызов
        /// </summary>
        /// <param name="taskId">Идентификатор задачи</param>
        /// <param name="params">Параметры</param>
        /// <param name="ctx">Контекст</param>
        /// <param name="indicator">Индикатор</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Результат</returns>
        public CallbackResult Call(
            long taskId,
            BaseParams @params,
            ExecutionContext ctx,
            IProgressIndicator indicator,
            CancellationToken ct)
        {
            var taskMan = this.container.Resolve<ITaskManager>();

            using (this.container.Using(taskMan))
            {
                taskMan.CreateTasks(new PeriodCloseTaskProvider_Step2(), @params, taskMan.GetParentId(taskId));
            }

            return new CallbackResult(false);
        }
        #endregion
    }
}