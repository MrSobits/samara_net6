namespace Bars.Gkh.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.ExecutionAction.ExecutionActionScheduler;
    
    using Castle.Windsor;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Абстрактный класс выполняемых действий
    /// </summary>
    public abstract class BaseExecutionAction : IExecutionAction
    {
        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public User User { get; set; } = null;

        /// <inheritdoc />
        public BaseParams ExecutionParams { get; set; }

        /// <inheritdoc />
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// Список идентификаторов действий для запуска после завершения текущего действия
        /// </summary>
        protected virtual IList<string> ActionCodeList { get; } = new List<string>();

        /// <inheritdoc />
        public void StartAfterSuccessActions()
        {
            try
            {
                this.Container.UsingForResolved<IExecutionActionService>((ioc, service) =>
                {
                    this.ActionCodeList.ForEach(x => service.CreateTaskFromExecutionAction(x, this.ExecutionParams));
                });
            }
            catch (Exception e)
            {
                this.Container.UsingForResolved<ILogger>((ioc, logManager) =>
                {
                    logManager.LogDebug("Ошибка при запуске задач", e);
                });
            }
        }

        /// <summary>
        /// Код для регистрации
        /// </summary>
        public virtual string Code => this.GetType().Name;

        /// <summary>
        /// Название для отображения
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Описание действия
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Действие
        /// </summary>
        public abstract Func<IDataResult> Action { get; }
    }
}