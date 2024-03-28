namespace Bars.Gkh.MigrationManager.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Application;
    using Bars.B4.IoC;
    using Bars.B4.Migrations;
    using Bars.B4.Modules.Tasks.Common.Service;

    using Castle.DynamicProxy;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Интерцептор перезапуска сервера расчётов после проведения миграций
    /// </summary>
    public class RestartCalcServerInterceptor : AbstractInterceptor<IMigrationManager>
    {
        private bool shouldRestart;

        /// <inheritdoc />
        protected override void OnBeforeProceed(IInvocation invocation)
        {
            this.shouldRestart = this.Service.ListMigration()
                    .Any(info =>
                        info.DatabaseStateInfos.Any(dbInfo =>
                            !new HashSet<string>(dbInfo.CurrentVersions)
                                .SetEquals(new HashSet<string>(dbInfo.NewVersions))));
        }

        /// <inheritdoc />
        protected override void OnAfterProceed(IInvocation invocation)
        {
            try
            {
                if ((bool)invocation.ReturnValue && this.shouldRestart)
                {
                    ApplicationContext.Current.Container.UsingForResolved<ITaskManager>((cnt, service) => service.RestartExecutor());
                }
            }
            catch (Exception e)
            {
                var log = ApplicationContext.Current.Container.Resolve<ILogger>();
                log.LogError(e, e.Message);
            }
            
        }
    }
}