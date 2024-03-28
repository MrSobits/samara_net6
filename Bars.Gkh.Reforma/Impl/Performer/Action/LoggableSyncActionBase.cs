namespace Bars.Gkh.Reforma.Impl.Performer.Action
{
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.Interface.Logger;

    using Castle.Windsor;

    /// <summary>
    ///     Базовый класс для выполнения логируемых действий сихронизации с Реформой ЖКХ
    /// </summary>
    /// <typeparam name="TParams">Тип входных параметров действия</typeparam>
    /// <typeparam name="TResult">Тип результат действия</typeparam>
    public abstract class LoggableSyncActionBase<TParams, TResult> : SyncActionBase<TParams, TResult>
    {
        #region Properties

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="syncProvider">
        ///     Провайдер синхронизации
        /// </param>
        /// <param name="parameters">
        ///     Параметры действия
        /// </param>
        protected LoggableSyncActionBase(IWindsorContainer container, ISyncProvider syncProvider, TParams parameters)
            : base(container, syncProvider, parameters)
        {
        }

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="syncProvider">
        ///     Провайдер синхронизации
        /// </param>
        protected LoggableSyncActionBase(IWindsorContainer container, ISyncProvider syncProvider)
            : base(container, syncProvider)
        {
        }

        /// <summary>
        /// Логгер действий
        /// </summary>
        protected ISyncLogger Logger
        {
            get
            {
                return this.SyncProvider.Logger;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Выполнение действия
        /// </summary>
        /// <returns>Результат действия</returns>
        public override SyncActionResult<TResult> Perform()
        {
            this.Logger.StartActionInvocation(this.Id, this.SerializedParameters);
            var result = base.Perform();
            this.Logger.EndActionInvocation(result);

            return result;
        }

        #endregion
    }
}