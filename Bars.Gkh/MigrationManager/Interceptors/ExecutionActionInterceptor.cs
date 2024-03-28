namespace Bars.Gkh.MigrationManager
{
    using Bars.B4.IoC;
    using Bars.B4.Migrations;
    using Bars.Gkh.ExecutionAction.ExecutionActionScheduler;
    using Bars.Gkh.MigrationManager.Interceptors;

    using Castle.DynamicProxy;
    using Castle.Windsor;

    /// <summary>
    /// Интерцептор вызова метода менеджера миграций
    /// </summary>
    public class ExecutionActionInterceptor : AbstractInterceptor<IMigrationManager>
    {
        private readonly IWindsorContainer container;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="container">Контейнер</param>
        public ExecutionActionInterceptor(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <inheritdoc />
        protected override void OnAfterProceed(IInvocation invocation)
        {
            var successMigrations = (bool)invocation.ReturnValue;

            if (successMigrations)
            {
                this.container.UsingForResolved<IExecutionActionService>((ioc, service) => service.RestoreJobs());
            }
        }
    }
}