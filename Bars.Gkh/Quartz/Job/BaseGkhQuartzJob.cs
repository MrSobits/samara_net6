namespace Bars.Gkh.Quartz.Job
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Bars.B4.Utils;
    using Bars.Gkh.Enums;

    using global::Quartz;

    /// <summary>
    /// Базовый класс для планируемой задачи
    /// </summary>
    public abstract class BaseGkhQuartzJob : IGkhQuartzJob
    {
        /// <inheritdoc />
        public virtual long TaskId { get; set; }

        /// <inheritdoc />
        public virtual JobKey JobKey { get; protected set; }

        /// <inheritdoc />
        public virtual TriggerKey TriggerKey { get; protected set; }

        protected virtual ISchedulableTask SchedulableTask { get; set; }

        protected CancellationTokenSource CancellationTokenSource { get; set; }

        protected CancellationToken CancellationToken => this.CancellationTokenSource?.Token ?? CancellationToken.None;

        /// <summary>
        /// Получить параметры задачи
        /// </summary>
        public abstract JobDataMap GetJobDataMap();

        /// <inheritdoc />
        public abstract void Execute(IJobExecutionContext context);

        protected void CreateCancellationTokenSource()
        {
            this.CancellationTokenSource = new CancellationTokenSource();
        }

        /// <inheritdoc />
        public virtual void Interrupt()
        {
            this.CancellationTokenSource?.Cancel();
        }

        /// <inheritdoc />
        public virtual IJobDetail GetJob()
        {
            return JobBuilder.Create(this.GetType())
                .WithIdentity(this.JobKey)
                .UsingJobData(this.GetJobDataMap())
                .Build();
        }

        /// <inheritdoc />
        public virtual ITrigger GetTrigger()
        {
            var triggerBuilder = TriggerBuilder.Create()
                .WithIdentity(this.TriggerKey);

            if (this.SchedulableTask.PeriodType == TaskPeriodType.NoPeriodicity)
            {
                if (this.SchedulableTask.StartNow)
                {
                    triggerBuilder = triggerBuilder.StartNow();
                }
                else
                {
                    var startDate = DateTime.Today
                        .AddHours(this.SchedulableTask.StartTimeHour)
                        .AddMinutes(this.SchedulableTask.StartTimeMinutes);
                    if (startDate <= DateTime.Now)
                    {
                        throw new Exception($"Указанное время запуска уже прошло: {startDate}");
                    }

                    triggerBuilder = triggerBuilder.StartAt(startDate.ToUniversalTime());
                }
            }
            else
            {
                var cronSchedule = CronScheduleBuilder.CronSchedule(this.GetCronExpression());
                triggerBuilder = triggerBuilder.WithSchedule(cronSchedule);
                if (this.SchedulableTask.StartDate.HasValue)
                {
                    var startDate = DateTime.UtcNow > this.SchedulableTask.StartDate.Value.ToUniversalTime()
                        ? DateTime.UtcNow
                        : this.SchedulableTask.StartDate.Value.ToUniversalTime();

                    triggerBuilder = triggerBuilder.StartAt(startDate);
                }
                if (this.SchedulableTask.EndDate.HasValue)
                {
                    triggerBuilder = triggerBuilder.EndAt(this.SchedulableTask.EndDate.Value.ToUniversalTime());
                }
            }

            return triggerBuilder.Build();
        }

        protected string GetCronExpression()
        {
            /*
            ┌───────────── секунды (0 - 59)
            | ┌───────────── минуты (0 - 59)
            | │ ┌────────────── часы (0 - 23)
            | │ │ ┌─────────────── день месяца (1 - 31)
            | │ │ │ ┌──────────────── месяц (1 - 12)
            | │ │ │ │ ┌───────────────── день недели (0 - 6) (0 - Вс, 6 - Сб; 7 - Вс)
            | │ │ │ │ │
            * * * * * *
             */
            var min = this.SchedulableTask.StartTimeMinutes.ToString();
            var hour = this.SchedulableTask.StartTimeHour.ToString();
            var dayOfMonth = "*";
            var month = "*";
            var dayOfWeek = "?";
            if (this.SchedulableTask.PeriodType == TaskPeriodType.Monthly)
            {
                dayOfMonth = this.GetDayOfMonth();
                month = this.GetMonth();
            }
            if (this.SchedulableTask.PeriodType == TaskPeriodType.Weekly)
            {
                dayOfMonth = "?";
                dayOfWeek = this.GetDayOfWeek();
            }

            return $"0 {min} {hour} {dayOfMonth} {month} {dayOfWeek}";
        }

        private string GetDayOfMonth()
        {
            if (this.SchedulableTask.StartDayOfWeekList.IsNotEmpty())
            {
                return "?";
            }

            if (this.SchedulableTask.StartDaysList.IsEmpty())
            {
                return "*";
            }

            var dayList = this.SchedulableTask.StartDaysList.OrderBy(x => x).ToList();

            if (dayList[0] == 0)
            {
                return "L";
            }

            return string.Join(",", dayList);
        }

        private string GetMonth()
        {
            if (this.SchedulableTask.StartMonthList.IsEmpty())
            {
                return "*";
            }

            return string.Join(",", this.SchedulableTask.StartMonthList);
        }

        private string GetDayOfWeek()
        {
            if (this.SchedulableTask.StartDayOfWeekList.IsEmpty() || this.SchedulableTask.StartDaysList.IsNotEmpty())
            {
                return "?";
            }

            return string.Join(",", this.SchedulableTask.StartDayOfWeekList.Select(x => BaseGkhQuartzJob.dayOfWeeks[x > 0 && x <=7 ? x : 0]));
        }

        private static Dictionary<int, string> dayOfWeeks = new Dictionary<int, string>
        {
            [0] = "?",
            [1] = "MON",
            [2] = "TUE",
            [3] = "WED",
            [4] = "THU",
            [5] = "FRI",
            [6] = "SAT",
            [7] = "SUN",
        };
    }
}