namespace Bars.Gkh.Reforma.Impl.Performer.Action
{
    using Bars.B4.Utils;

    /// <summary>
    ///     Результат выполнения действия
    /// </summary>
    public class SyncActionResult
    {
        #region Public Properties

        /// <summary>
        ///     Результат действия
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        ///     Детали ошибки
        /// </summary>
        public ErrorDetails ErrorDetails { get; set; }

        /// <summary>
        ///     Признак успешного исполнения действия
        /// </summary>
        public bool Success { get; set; }

        #endregion
    }

    /// <summary>
    ///     Обобщенный результат выполнения действия
    /// </summary>
    /// <typeparam name="TResult">Тип результата действия</typeparam>
    public class SyncActionResult<TResult> : SyncActionResult
    {
        #region Public Properties

        /// <summary>
        ///     Результат действия
        /// </summary>
        public new TResult Data
        {
            get
            {
                return base.Data != null ? (TResult)base.Data : default(TResult);
            }

            set
            {
                base.Data = value;
            }
        }

        #endregion
    }
}