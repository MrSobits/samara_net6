namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ActionIsolated
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    public class ActActionIsolatedViewModel : BaseViewModel<ActActionIsolated>
    {
        public override IDataResult Get(IDomainService<ActActionIsolated> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var obj = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            var docChildrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var taskActionIsolatedDomain = this.Container.Resolve<IDomainService<TaskActionIsolated>>();
            var inspectorDomain = this.Container.Resolve<IDocumentGjiInspectorService>();
            var inspectors = string.Empty;
            var inspectorsIds = string.Empty;
            KindAction? kindAction = null;

            using (this.Container.Using(inspectorDomain, docChildrenDomain, taskActionIsolatedDomain))
            {
                var dataInspectors = inspectorDomain.GetInspectorsByDocumentId(id)
                    .Select(x => new
                    {
                        InspectorId = x.Inspector.Id,
                        x.Inspector.Fio
                    })
                    .ToList();

                inspectors = string.Join(", ", dataInspectors.Select(x=>x.Fio));
                inspectorsIds = string.Join(", ", dataInspectors.Select(x => x.InspectorId));

                var taskActionId = docChildrenDomain.GetAll()
                    .FirstOrDefault(x => x.Children == obj && x.Parent.TypeDocumentGji == TypeDocumentGji.TaskActionIsolated)?.Parent?.Id;

                if (taskActionId != null)
                {
                    kindAction = taskActionIsolatedDomain.Get(taskActionId).KindAction;
                }
            }
            
            return obj != null
                ? new BaseDataResult(
                    new
                    {
                        obj.Id,
                        obj.Area,
                        obj.DocumentDate,
                        obj.DocumentNum,
                        obj.DocumentNumber,
                        obj.DocumentPlaceFias,
                        obj.LiteralNum,
                        obj.DocumentSubNum,
                        obj.DocumentYear,
                        obj.Flat,
                        DocumentTime = obj.DocumentTime.HasValue ? obj.DocumentTime.Value.ToString("HH:mm") : string.Empty,
                        obj.AcquaintState,
                        obj.AcquaintedDate,
                        obj.RefusedToAcquaintPerson,
                        obj.AcquaintedPerson,
                        obj.State,
                        Inspectors = inspectors,
                        InspectorIds = inspectorsIds,
                        KindAction = kindAction
                    })
                : new BaseDataResult();
        }
    }
}