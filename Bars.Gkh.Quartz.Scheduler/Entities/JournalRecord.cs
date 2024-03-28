namespace Bars.Gkh.Quartz.Scheduler.Entities
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// Запись журнала выполнения триггера
    /// </summary>
    public class JournalRecord : BaseEntity
    {
        /// <summary>
        /// Ссылка на триггер
        /// </summary>
        public virtual Trigger Trigger { get; set; }

        /// <summary>
        /// Время начала выполнения
        /// </summary>
        public virtual DateTime StartTime { get; set; }

        /// <summary>
        /// Время окончания выполнения
        /// </summary>
        public virtual DateTime EndTime { get; set; }

        /// <summary>
        /// Результат выполнения
        /// </summary>
        public virtual byte[] Result { get; set; }

        /// <summary>
        /// Протокол выполнения
        /// </summary>
        public virtual byte[] Protocol { get; set; }

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public virtual string Message { get; set; }

        /// <summary>
        /// Выполнение прервано
        /// </summary>
        public virtual bool Interrupted { get; set; }
    }
}
