namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.PreventiveAction
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    public class PreventiveActionTaskItemViewModel : BaseViewModel<PreventiveActionTaskItem>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<PreventiveActionTaskItem> domainService, BaseParams baseParams)
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