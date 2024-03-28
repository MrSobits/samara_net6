namespace Bars.Gkh.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities.Administration.ExecutionAction;
    using Bars.Gkh.ExecutionAction.ExecutionActionScheduler;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Результаты выполнения действий
    /// </summary>
    public class ExecutionActionResultViewModel : BaseViewModel<ExecutionActionResult>
    {
        public IExecutionActionInfoService ExecutionActionInfoService { get; set; }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<ExecutionActionResult> domainService, BaseParams baseParams)
        {
            return domainService.GetAll()
                .Where(x => x.ObjectCreateDate > DateTime.Now.AddMonths(-1))
                .Select(x => new
                {
                    x.Id,
                    x.Task.ActionCode,
                    x.StartDate,
                    x.EndDate,
                    x.Result,
                    x.Status
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    Name = this.ExecutionActionInfoService.GetInfo(x.ActionCode)?.Name ?? x.ActionCode,
                    Description = this.ExecutionActionInfoService.GetInfo(x.ActionCode)?.Description ?? string.Empty,
                    x.StartDate,
                    x.EndDate,
                    x.Result,
                    x.Status,
                    Duration = x.EndDate.HasValue
                        ? x.EndDate - x.StartDate
                        : new TimeSpan(0)
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}