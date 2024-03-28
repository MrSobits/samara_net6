namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Modules.TaskManager.Contracts.Entities;
    using B4.Utils;
    using Entities;

    public class ComputingProcessViewModel: BaseViewModel<ComputingProcess>
    {
        private readonly IDomainService<TaskResult> _taskResultDomain;

        public ComputingProcessViewModel(IDomainService<TaskResult> taskResultDomain)
        {
            _taskResultDomain = taskResultDomain;
        }

        public override IDataResult List(IDomainService<ComputingProcess> domainService, BaseParams baseParams)
        {
            var userId = Container.Resolve<IUserIdentity>().UserId;

            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .WhereIf(userId > 0, x => x.Issuer.Id == userId)
                .Filter(loadParams, Container);

            int totalCount = data.Count();
            List<ComputingProcess> computingProcesses = data.Order(loadParams).Paging(loadParams).ToList();

            var taskIds = computingProcesses.Select(x => x.TaskId).ToList();

            var tasks =
                _taskResultDomain.GetAll()
                    .Where(x => taskIds.Contains(x.TaskId))
                    .ToList()
                    .GroupBy(x => x.TaskId)
                    .ToDictionary(x => x.Key, x => x.First());

            var result = computingProcesses.Select(x => new
            {
                x.Id,
                x.Issuer,
                x.Name,
                x.ObjectCreateDate,
                x.Status,
                x.TaskId,
                x.Type,
                Data = tasks.ContainsKey(x.TaskId) ? tasks[x.TaskId] : null
            });

            return new ListDataResult(result, totalCount);
        }
    }
}
