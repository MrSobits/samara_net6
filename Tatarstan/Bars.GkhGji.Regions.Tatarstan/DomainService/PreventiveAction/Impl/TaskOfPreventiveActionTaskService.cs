namespace Bars.GkhGji.Regions.Tatarstan.DomainService.PreventiveAction.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    using Castle.Windsor;

    public class TaskOfPreventiveActionTaskService : ITaskOfPreventiveActionTaskService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult AddTasks(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");
            var tasksIds = baseParams.Params.GetAs<long[]>("ids");
            var taskOfPreventiveActionTaskDomain = this.Container.ResolveDomain<TaskOfPreventiveActionTask>();

            using (this.Container.Using(taskOfPreventiveActionTaskDomain))
            {
                var tasksToSave = tasksIds.Select(id => new TaskOfPreventiveActionTask()
                {
                    PreventiveActionTask = new PreventiveActionTask() { Id = documentId },
                    TasksPreventiveMeasures = new TasksPreventiveMeasures() { Id = id }
                });

                TransactionHelper.InsertInManyTransactions(this.Container, tasksToSave);
            }

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult GetAllTasks(BaseParams baseParams)
        {
            var domainService = this.Container.ResolveDomain<TasksPreventiveMeasures>();

            using (this.Container.Using(domainService))
            {
                return domainService.GetAll()
                    .Select(x=> new
                    {
                        x.Id,
                        x.Name
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), Container);
            }
        }
    }
}