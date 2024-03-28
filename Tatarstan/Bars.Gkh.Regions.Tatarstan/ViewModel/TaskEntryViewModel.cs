namespace Bars.Gkh.Regions.Tatarstan.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Utils;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.Tasks.Common.Entities;

    public class TaskEntryViewModel : BaseViewModel<TaskEntry>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<TaskEntry> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var userDomain = this.Container.ResolveDomain<User>();
            using (this.Container.Using(userDomain))
            {
                return domainService.GetAll()
                    .Join(userDomain.GetAll(),
                        x => x.UserId,
                        y => y.Id,
                        (x, y) => new
                        {
                            TaskEntry = x,
                            UserLogin = y.Login
                        })
                    .Where(x => x.TaskEntry.Parent != null)
                    .Select(x => new
                    {
                        x.TaskEntry.Id,
                        ParentId = x.TaskEntry.Parent.Id,
                        x.TaskEntry.Name,
                        x.TaskEntry.Description,
                        x.TaskEntry.Status,
                        x.UserLogin,
                        CreateDate = x.TaskEntry.ObjectCreateDate,
                        Duration = TimeSpan.FromMilliseconds(x.TaskEntry.ElapsedMilliseconds),
                        x.TaskEntry.Progress,
                        x.TaskEntry.Percentage
                    })
                    .OrderByDescending(x => x.CreateDate)
                    .ThenBy(x => x.Id)
                    .ToListDataResult(loadParams, this.Container);
            }
        }
    }
}