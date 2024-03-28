namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.PreventiveAction
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    public class TaskOfPreventiveActionTaskViewModel : BaseViewModel<TaskOfPreventiveActionTask>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<TaskOfPreventiveActionTask> domainService, BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");
            
            return domainService.GetAll().Where(x=>x.PreventiveActionTask.Id == documentId)
                .Select(x=> new
                {
                    x.Id,
                    x.TasksPreventiveMeasures.Name
                })
                .ToListDataResult(baseParams.GetLoadParam(), Container);
        }
    }
}