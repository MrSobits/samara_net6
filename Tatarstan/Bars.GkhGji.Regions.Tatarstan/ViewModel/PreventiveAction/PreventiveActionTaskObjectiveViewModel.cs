namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.PreventiveAction
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    public class PreventiveActionTaskObjectiveViewModel : BaseViewModel<PreventiveActionTaskObjective>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<PreventiveActionTaskObjective> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId("documentId");

            var result = domainService.GetAll()
                .Where(x => x.PreventiveActionTask.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.ObjectivesPreventiveMeasure.Name
                });

            return new BaseDataResult(result);
        }
    }
}
