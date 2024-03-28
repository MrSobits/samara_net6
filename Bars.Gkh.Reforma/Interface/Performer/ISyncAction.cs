namespace Bars.Gkh.Reforma.Interface.Performer
{
    using Bars.Gkh.Reforma.Impl.Performer.Action;

    /// <summary>
    ///     Интерфейс действия синхронизации с Реформой ЖКХ
    /// </summary>
    public interface ISyncAction
    {
        #region Public Properties

        /// <summary>
        ///     Идентификатор действия
        /// </summary>
        string Id { get; }

        /// <summary>
        ///     Параметры действия
        /// </summary>
        object Parameters { get; set; }

        /// <summary>
        ///     Сериализованные в JSON параметры. Геттер сериализует текущие параметры, сеттер - десериализует
        /// </summary>
        string SerializedParameters { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Выполнение действия
        /// </summary>
        /// <returns>Результат действия</returns>
        SyncActionResult Perform();

        #endregion
    }

    /// <summary>
    ///     Обобщенный интерфейс действия синхронизации с Реформой ЖКХ
    /// </summary>
    /// <typeparam name="TParam">Тип входных параметров</typeparam>
    /// <typeparam name="TResult">Тип результата</typeparam>
    public interface ISyncAction<TParam, TResult> : ISyncAction
    {
        #region Public Properties

        /// <summary>
        ///     Входные параметры действия
        /// </summary>
        new TParam Parameters { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Выполнение действия
        /// </summary>
        /// <returns>Результат действия</returns>
        new SyncActionResult<TResult> Perform();

        #endregion
    }
}