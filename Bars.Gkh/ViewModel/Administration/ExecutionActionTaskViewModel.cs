namespace Bars.Gkh.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Administration.ExecutionAction;
    using Bars.Gkh.Enums;
    using Bars.Gkh.ExecutionAction.ExecutionActionScheduler;
    using Bars.Gkh.Utils;

    public class ExecutionActionTaskViewModel : BaseViewModel<ExecutionActionTask>
    {
        public IExecutionActionInfoService ExecutionActionInfoService { get; set; }
        public IExecutionActionScheduler ExecutionActionScheduler { get; set; }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<ExecutionActionTask> domainService, BaseParams baseParams)
        {
            return domainService.GetAll()
                .Where(x => !x.IsDelete)
                .Select(x => new
                    {
                        x.Id,
                        x.User.Login,
                        x.ObjectCreateDate,
                        x.StartDate,
                        x.EndDate,
                        x.StartTimeHour,
                        x.StartTimeMinutes,
                        x.PeriodType,
                        x.StartDayOfWeekList,
                        x.StartMonthList,
                        x.ActionCode
                    })
                .AsEnumerable()
                .Select(x => new
                    {
                        x.Id,
                        x.Login,
                        TriggerName =
                        this.GetTriggerName(x.PeriodType, x.StartTimeHour, x.StartTimeMinutes, x.StartDayOfWeekList, x.StartMonthList),
                        CreateDate = x.ObjectCreateDate,
                        x.StartDate,
                        x.EndDate,
                        Name = this.ExecutionActionInfoService.GetInfo(x.ActionCode)?.Name ?? x.ActionCode,
                        this.ExecutionActionInfoService.GetInfo(x.ActionCode)?.Description
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }

        /// <inheritdoc />
        public override IDataResult Get(IDomainService<ExecutionActionTask> domainService, BaseParams baseParams)
        {
            var task = domainService.Get(baseParams.Params.GetAsId());
            return new BaseDataResult(new
            {
                task.Id,
                task.User,
                task.StartDate,
                task.EndDate,
                task.PeriodType,
                CreateDate = task.ObjectCreateDate,
                task.StartNow,
                task.StartTimeHour,
                task.StartTimeMinutes,
                task.ActionCode,
                task.IsDelete,
                task.StartDayOfWeekList,
                task.StartMonthList,
                task.StartDaysList,
                task.BaseParams,
                Name = this.ExecutionActionInfoService.GetInfo(task.ActionCode)?.Name ?? task.ActionCode,
                this.ExecutionActionInfoService.GetInfo(task.ActionCode)?.Description
            });
        }

        private string GetTriggerName(TaskPeriodType periodType,
            int startTimeHour,
            int startTimeMinutes,
            IList<byte> startDayOfWeekList,
            IList<byte> startMonthList)
        {
            switch (periodType)
            {
                case TaskPeriodType.NoPeriodicity:
                    return $"Одноразовый запуск. Время запуска: {startTimeHour:D2}:{startTimeMinutes:D2}";

                case TaskPeriodType.Daily:
                    return $"Ежедневно. Время запуска: {startTimeHour:D2}:{startTimeMinutes:D2}";

                case TaskPeriodType.Weekly:
                    return $"Еженедельно: {startDayOfWeekList.AggregateWithSeparator(this.GetDayOfWeeName, ", ")}. " +
                        $"Время запуска {startTimeHour:D2}:{startTimeMinutes:D2}";

                case TaskPeriodType.Monthly:
                    return $"Ежемесячно: {startMonthList.AggregateWithSeparator(this.GetMonthName, ", ")}. " +
                        $"Время запуска {startTimeHour:D2}:{startTimeMinutes:D2}";

                default:
                    throw new InvalidEnumArgumentException(nameof(periodType), (int)periodType, periodType.GetType());
            }
        }

        private string GetDayOfWeeName(byte dayOfWeek)
        {
            if (dayOfWeek > 7)
            {
                throw new ArgumentOutOfRangeException(nameof(dayOfWeek));
            }

            // 1 января 1 г. н.э. был понедельник
            return new DateTime(1, 1, dayOfWeek).ToString("ddd");
        }

        private string GetMonthName(byte monthNumber)
        {
            return new DateTime(1, monthNumber, 1).ToString("MMMM");
        }
    }
}