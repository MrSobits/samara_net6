namespace Bars.Gkh.Entities
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Quartz.Job;

    /// <summary>
    /// Базовый класс для планируемой задачи
    /// </summary>
    public class BaseSchedulableTask : BaseEntity, ISchedulableTask
    {
        /// <summary>
        /// Дата начала действия
        /// </summary>
        public virtual DateTime? StartDate { get; set; }

        /// <summary>
        /// Дата окончания действия
        /// </summary>
        public virtual DateTime? EndDate { get; set; }

        /// <summary>
        /// Периодичность запуска
        /// </summary>
        public virtual TaskPeriodType PeriodType { get; set; }

        /// <summary>
        /// Запустить сейчас
        /// <para>Не хранимое</para>
        /// </summary>
        public virtual bool StartNow { get; set; }

        /// <summary>
        /// Час запуска
        /// </summary>
        public virtual int StartTimeHour { get; set; }

        /// <summary>
        /// Минуты запуска
        /// </summary>
        public virtual int StartTimeMinutes { get; set; }

        /// <summary>
        /// Дни недели запуска
        /// </summary>
        public virtual IList<byte> StartDayOfWeekList { get; set; }

        /// <summary>
        /// Месяцы запуска
        /// </summary>
        public virtual IList<byte> StartMonthList { get; set; }

        /// <summary>
        /// Числа месяца запуска
        /// <para>
        /// 0 - последний день месяца
        /// </para>
        /// </summary>
        public virtual IList<byte> StartDaysList { get; set; }
    }
}