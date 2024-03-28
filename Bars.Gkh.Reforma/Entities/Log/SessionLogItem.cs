namespace Bars.Gkh.Reforma.Entities.Log
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Reforma.Enums;

    /// <summary>
    /// Запись лога о сессии синхронизации
    /// </summary>
    public class SessionLogItem : BaseEntity
    {
        /// <summary>
        /// Идентификатор сессии
        /// </summary>
        public virtual string SessionId { get; set; }

        /// <summary>
        /// Время открытия сессии
        /// </summary>
        public virtual DateTime StartTime { get; set; }

        /// <summary>
        /// Время закрытия сессии
        /// </summary>
        public virtual DateTime? EndTime { get; set; }

        /// <summary>
        /// Тип интеграции
        /// </summary>
        public virtual TypeIntegration TypeIntegration { get; set; }
    }
}