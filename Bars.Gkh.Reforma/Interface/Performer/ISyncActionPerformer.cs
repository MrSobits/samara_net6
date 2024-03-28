namespace Bars.Gkh.Reforma.Interface.Performer
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.Reforma.Impl.Performer.Action;

    /// <summary>
    /// Делегат описывающий колбэк, вызываемый после завершения действия
    /// </summary>
    /// <typeparam name="TResult">
    /// Тип результата действия
    /// </typeparam>
    /// <param name="result">
    /// Результат действия
    /// </param>
    public delegate void PerformerCallback<TResult>(SyncActionResult<TResult> result);

    /// <summary>
    /// Делегат описывающий колбэк, вызываемый после завершения действия
    /// </summary>
    /// <param name="result">
    /// Результат действия
    /// </param>
    public delegate void PerformerCallback(SyncActionResult result);

    /// <summary>
    ///     Планировщик действий синхронизации
    /// </summary>
    public interface ISyncActionPerformer
    {
        #region Public Methods and Operators

        /// <summary>
        /// Добавление действия в очередь
        /// </summary>
        /// <typeparam name="TParam">
        /// Тип параметров действия
        /// </typeparam>
        /// <typeparam name="TResult">
        /// Тип результата действия
        /// </typeparam>
        /// <param name="action">
        /// Экземляр действия
        /// </param>
        /// <returns>
        /// Настройщик запланированного действия
        /// </returns>
        IQueuedActionConfigurator<TParam, TResult> AddToQueue<TParam, TResult>(ISyncAction<TParam, TResult> action);

        /// <summary>
        /// Добавление действия в очередь
        /// </summary>
        /// <param name="action">
        /// Экземпляр действия
        /// </param>
        /// <returns>
        /// Настройщик запланированного действия
        /// </returns>
        IQueuedActionConfigurator AddToQueue(ISyncAction action);

        /// <summary>
        /// Добавление действия в очередь
        /// </summary>
        /// <typeparam name="TAction">
        /// Тип действия
        /// </typeparam>
        /// <returns>
        /// Настройщик запланированного действия
        /// </returns>
        IQueuedActionConfigurator AddToQueue<TAction>() where TAction : ISyncAction;

        /// <summary>
        /// Добавление действия в очередь
        /// </summary>
        /// <typeparam name="TAction">
        /// Тип действия
        /// </typeparam>
        /// <typeparam name="TParam">
        /// Тип параметров действия
        /// </typeparam>
        /// <typeparam name="TResult">
        /// Тип результата действия
        /// </typeparam>
        /// <returns>
        /// Настройщик запланированного действия
        /// </returns>
        IQueuedActionConfigurator<TParam, TResult> AddToQueue<TAction, TParam, TResult>() where TAction : ISyncAction<TParam, TResult>;

        /// <summary>
        ///     Запуск исполнения очереди действий
        /// </summary>
        void Perform();

        void WhenAll(IEnumerable<IQueuedActionConfigurator> actions, Action<IEnumerable<SyncActionResult>> callback);

        #endregion
    }
}