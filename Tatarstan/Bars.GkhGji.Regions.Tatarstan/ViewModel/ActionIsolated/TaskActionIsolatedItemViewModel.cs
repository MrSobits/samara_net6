namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ActionIsolated
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using System.Linq;

    using Bars.Gkh.Domain;

    public class TaskActionIsolatedItemViewModel : BaseViewModel<TaskActionIsolatedItem>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<TaskActionIsolatedItem> domainService, BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");

            return domainService
                .GetAll()
                .WhereIf(documentId > 0, x => x.Task.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.Task,
                    x.Item.Name
                })
                .ToListDataResult(this.GetLoadParam(baseParams), this.Container);
        }
    }
}
