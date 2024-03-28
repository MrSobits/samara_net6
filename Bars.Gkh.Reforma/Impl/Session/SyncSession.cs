namespace Bars.Gkh.Reforma.Impl.Session
{
    using System;

    using Bars.Gkh.Reforma.Enums;
    using Bars.Gkh.Reforma.Interface.Session;

    /// <summary>
    ///     Сессия синхронизации
    /// </summary>
    public class SyncSession : ISyncSession
    {
        #region Fields

        private readonly object lockObject = new object();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Конструктор
        /// </summary>
        public SyncSession(TypeIntegration typeIntegration)
            : this(Guid.NewGuid(), false)
        {
            this.TypeIntegration = typeIntegration;
        }

        /// <summary>
        /// Конструктор, переоткрывающий сессию
        /// </summary>
        /// <param name="sessionId">
        /// Идентификатор переоткрываемой сессии
        /// </param>
        public SyncSession(Guid sessionId)
            : this(sessionId, true)
        {
        }

        /// <summary>
        /// Приватный конструктор
        /// </summary>
        /// <param name="sessionId">
        /// Идентификатор сессии
        /// </param>
        /// <param name="resurrected">
        /// Признак переоткрытия
        /// </param>
        private SyncSession(Guid sessionId, bool resurrected)
        {
            this.SessionId = sessionId;
            this.IsOpen = true;
            this.StartTime = DateTime.UtcNow;
            this.Resurrected = resurrected;
        }

        #endregion

        #region Public Events

        /// <summary>
        ///     Событие, возникающее в момент закрытия сессии
        /// </summary>
        public event Action<ISyncSession> SessionClosed;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Признак открытости
        /// </summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        /// Признак переоткрытия сессии
        /// </summary>
        public bool Resurrected { get; private set; }

        /// <summary>
        ///     Идентификатор сессии
        /// </summary>
        public Guid SessionId { get; private set; }

        /// <summary>
        ///     Время открытия сессии
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        ///     Тип интеграции
        /// </summary>
        public TypeIntegration TypeIntegration { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Закрытие сессии
        /// </summary>
        public void Close()
        {
            if (this.IsOpen)
            {
                lock (this.lockObject)
                {
                    if (this.IsOpen)
                    {
                        Action<ISyncSession> sessionClosed = this.SessionClosed;
                        if (sessionClosed != null)
                        {
                            sessionClosed(this);
                        }

                        this.IsOpen = false;
                    }
                }
            }
        }

        #endregion
    }
}