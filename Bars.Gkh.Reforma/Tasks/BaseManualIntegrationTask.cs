namespace Bars.Gkh.Reforma.Tasks
{
    using System;

    using Bars.B4.IoC;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Utils;
    using Bars.Gkh.Reforma.Enums;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.Interface.Performer;

    using Castle.MicroKernel;
    using Castle.MicroKernel.Lifestyle;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Задача обновления профиля компании
    /// </summary>
    public abstract class BaseManualIntegrationTask<TParams> : BaseTask
    {
        /// <summary>
        /// Параметры задачи
        /// </summary>
        protected TParams TaskParams;
        
        /// <summary>
        /// Провайдер синхронизации
        /// </summary>
        private ISyncProvider SyncProvider;

        /// <summary>
        /// Планировщик задач
        /// </summary>
        protected ISyncActionPerformer Performer => this.SyncProvider.Performer;

        /// <summary>Выполнение задачи</summary>
        /// <param name="params">
        /// Параметры исполнения задачи.
        /// При вызове из планировщика передаются параметры из JobDataMap
        /// и контекст исполнения в параметре JobContext
        /// </param>
        public override void Execute(DynamicDictionary @params)
        {
            this.TaskParams = this.ExtractParamsFromArgs(@params);

            try
            {
                using (this.Container.BeginScope())
                {
                    try
                    {
                        var argument = new Arguments { { "typeIntegration", TypeIntegration.Selection } };
                        this.SyncProvider = this.Container.Resolve<ISyncProvider>(argument);
                        this.Execute();
                    }
                    catch (Exception exception)
                    {
                        this.SyncProvider.Logger.SetException(exception);
                        throw;
                    }
                    finally
                    {
                        if (this.SyncProvider != null)
                        {
                            this.SyncProvider.Close();
                            this.Container.Release(this.SyncProvider);
                            this.SyncProvider = null;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.Container.UsingForResolved<ILogger>((container, manager) => manager.LogError(exception.ToString(), exception));
            }
        }

        /// <summary>
        /// Выполяемое действие
        /// </summary>
        protected abstract void Execute();

        /// <summary>
        /// Извлечь параметры задачи
        /// </summary>
        /// <param name="params">Словарь параметров</param>
        /// <returns>Параметры</returns>
        protected abstract TParams ExtractParamsFromArgs(DynamicDictionary @params);
    }
}