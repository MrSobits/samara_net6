namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ActionIsolated
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    public class TaskActionIsolatedRealityObjectViewModel : BaseViewModel<TaskActionIsolatedRealityObject>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<TaskActionIsolatedRealityObject> domainService, BaseParams baseParams)
        {
            var taskActionId = baseParams.Params.GetAsId("documentId");

            return domainService.GetAll()
                .Where(x => x.Task.Id == taskActionId)
                .Select(x => new
                {
                    x.Id,
                    InspectionGji = x.Task.Inspection,
                    Municipality = x.RealityObject.Municipality.Name,
                    RealityObjectId = x.RealityObject.Id,
                    x.RealityObject,
                    x.RealityObject.Address,
                    Area = x.RealityObject.AreaMkd
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}