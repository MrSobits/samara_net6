namespace Bars.Gkh.Reforma.Entities.Log
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Запись лога о произведенной в рамках сессии операции
    /// </summary>
    public class ActionLogItem : BaseEntity
    {
        /// <summary>
        /// Сессия
        /// </summary>
        public virtual SessionLogItem Session { get; set; }

        /// <summary>
        /// Имя действия
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Параметры вызова действия
        /// </summary>
        public virtual byte[] Parameters { get; set; }

        /// <summary>
        /// Детали действия
        /// </summary>
        public virtual string Details { get; set; }

        /// <summary>
        /// Признак успешности
        /// </summary>
        public virtual bool Success { get; set; }

        /// <summary>
        /// Код ошибки
        /// </summary>
        public virtual string ErrorCode { get; set; }

        /// <summary>
        /// Название ошибки
        /// </summary>
        public virtual string ErrorName { get; set; }

        /// <summary>
        /// Описание ошибки
        /// </summary>
        public virtual string ErrorDescription { get; set; }

        /// <summary>
        /// Время запроса
        /// </summary>
        public virtual DateTime RequestTime { get; set; }

        /// <summary>
        /// Время ответа
        /// </summary>
        public virtual DateTime ResponseTime { get; set; }

        /// <summary>
        /// Отправленные/полученные пакеты
        /// </summary>
        public virtual FileInfo Packets { get; set; }
    }
}