namespace Bars.Gkh.Reforma.Interface.Session
{
    using System;

    using Bars.Gkh.Reforma.Enums;

    /// <summary>
    /// Сессия синхронизации
    /// </summary>
    public interface ISyncSession
    {
        #region Public Events

        /// <summary>
        /// Событие, вызываемое при закрытии сессии
        /// </summary>
        event Action<ISyncSession> SessionClosed;

        #endregion

        #region Public Properties

        /// <summary>
        /// Признак открытости сессии
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// Признак переоткрытия сессии
        /// </summary>
        bool Resurrected { get; }

        /// <summary>
        /// Идентификатор сессии
        /// </summary>
        Guid SessionId { get; }

        /// <summary>
        /// Время открытия сессии
        /// </summary>
        DateTime StartTime { get; }

        /// <summary>
        ///     Тип интеграции
        /// </summary>
        TypeIntegration TypeIntegration { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Закрытие сессии
        /// </summary>
        void Close();

        #endregion
    }
}