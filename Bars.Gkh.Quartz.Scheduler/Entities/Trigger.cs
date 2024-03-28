namespace Bars.Gkh.Quartz.Scheduler.Entities
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// Хранимый триггер
    /// </summary>
    public class Trigger : BaseEntity
    {
        /// <summary>
        /// Имя класса, реализующего выполнение триггера
        /// </summary>
        public virtual string ClassName { get; set; }

        /// <summary>
        /// Ключ объекта Trigger в хранилище планировщика задач
        /// </summary>
        public virtual string QuartzTriggerKey { get; set; }

        /// <summary>
        /// Начальные параметры выполненя
        /// </summary>
        public virtual byte[] StartParams { get; set; }

        /// <summary>
        /// Количество повторов
        /// </summary>
        public virtual int RepeatCount { get; set; }

        /// <summary>
        /// Интервал в секундах
        /// </summary>
        public virtual int Interval { get; set; }

        /// <summary>
        /// Время начала выполнения
        /// </summary>
        public virtual DateTime StartTime { get; set; }

        /// <summary>
        /// Время окончания выполнения
        /// </summary>
        public virtual DateTime EndTime { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public virtual string UserName { get; set; }
    }
}
