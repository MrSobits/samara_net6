namespace Bars.Gkh.Quartz.Job
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.Enums;

    /// <summary>
    /// Интерфейс планируемой задачи
    /// </summary>
    public interface ISchedulableTask
    {
        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        long Id { get; set; }

        /// <summary>
        /// Дата начала действия
        /// </summary>
        DateTime? StartDate { get; set; }

        /// <summary>
        /// Дата окончания действия
        /// </summary>
        DateTime? EndDate { get; set; }

        /// <summary>
        /// Периодичность запуска
        /// </summary>
        TaskPeriodType PeriodType { get; set; }

        /// <summary>
        /// Запустить сейчас
        /// </summary>
        bool StartNow { get; set; }

        /// <summary>
        /// Час запуска
        /// </summary>
        int StartTimeHour { get; set; }

        /// <summary>
        /// Минуты запуска
        /// </summary>
        int StartTimeMinutes { get; set; }

        /// <summary>
        /// Дни недели запуска
        /// </summary>
        IList<byte> StartDayOfWeekList { get; set; }

        /// <summary>
        /// Месяцы запуска
        /// </summary>
        IList<byte> StartMonthList { get; set; }

        /// <summary>
        /// Числа месяца запуска
        /// <para>
        /// 0 - последний день месяца
        /// </para>
        /// </summary>
        IList<byte> StartDaysList { get; set; }
    }
}