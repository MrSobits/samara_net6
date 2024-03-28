namespace Bars.Gkh.Quartz.Job
{
    using System;

    public interface IGkhQuartzJobInfo
    {
        /// <summary>
        /// Имя задачи
        /// </summary>
        string JobName { get; set; }

        /// <summary>
        /// Группа задачи
        /// </summary>
        string JobGroup { get; set; }

        /// <summary>
        /// Следующее время запуска
        /// </summary>
        DateTime? NextFireTime { get; set; }

        /// <summary>
        /// Предыдущее время запуска
        /// </summary>
        DateTime? PreviousFireTime { get; set; }
    }
}